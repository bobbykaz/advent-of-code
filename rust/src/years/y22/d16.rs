use std::{collections::{HashMap, HashSet, VecDeque}, fmt};

use crate::util;

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d16.txt");

    let mut all_valves: Vec<Valve> = vec![];
    for i in 0..lines.len() {
        let v = parse_valve(&lines[i], i);
        all_valves.push(v);
    }

    let non_empty_valve_idx: Vec<usize> = (0..all_valves.len()).filter(|v|all_valves[*v].fr != 0).collect();
    let mut vmap: HashMap<String,usize> = HashMap::new();
    for i in 0..all_valves.len() {
        vmap.insert(all_valves[i].key.clone(), i);
    }

    println!("Non empty valves: {:?}", non_empty_valve_idx);

    let vs = VS { av: all_valves, vm: vmap};

    let mut shortest_dist_grid: Vec<Vec<u32>> = vec![];
    for n in 0..vs.av.len() {
        shortest_dist_grid.push(vec![]);
        if non_empty_valve_idx.contains(&n) || vs.av[n].key == "AA" {
            for to in 0..vs.av.len() {
                if non_empty_valve_idx.contains(&to) {
                    let dist_to = shortest_path(&vs, n, to);
                    shortest_dist_grid[n].push(dist_to);
                    println!("Shortest path from {} to {} is {}", n, to, dist_to);
                } else {
                    shortest_dist_grid[n].push(99);
                }
            }
        }

    }

    print_valves(&vs);
    p1_dyn(&vs, &shortest_dist_grid, &non_empty_valve_idx);
}

fn shortest_path(vs:&VS, from_i:usize, to_i:usize) -> u32 {
    let mut seen: HashSet<usize> = HashSet::new();
    let mut queue: VecDeque<(usize,u32)> = VecDeque::new();

    seen.insert(from_i);
    queue.push_back((from_i,0));

    while queue.len() > 0 {
        let (current_idx, current_dist) = queue.pop_front().expect("queue must have an item");
        
        if current_idx == to_i {
            return current_dist;
        }
        
        let next_keys: Vec<String> = vs.av[current_idx].next.iter()
                        .map(|p| p.key.clone())
                        .collect();
        let next_idx: Vec<usize> = next_keys.iter()
                        .map(|k| vs.find(k).idx)
                        .collect();
        for idx in next_idx {
            if !seen.contains(&idx) {
                seen.insert(idx);
                queue.push_back((idx,current_dist+1));
            }
        }
    }

    panic!("Could not find path from {} to {}",from_i, to_i);
}

fn print_valves(vs: &VS) {
    for v in vs.av.iter() {
        if v.next.len() >0 {
            let mut kids = String::new();
            for p in v.next.iter() {
                kids.push_str(&p.to_string());
            }
            println!("{} -> {}", v.key, kids);
        }
    }
}

fn p1_dyn(vs: &VS, sp: &Vec<Vec<u32>>, target_valve_idxs:&Vec<usize>) -> u32 {
    let start = vs.find(&String::from("AA"));
    let start_ps = PS{time:30, cr:0, total:0};

    let max = p1_dyn_r(vs, sp, target_valve_idxs, start.idx, start_ps);
    println!("Max pressure:{max}");
    0
}

fn p1_dyn_r(vs: &VS, sp: &Vec<Vec<u32>>, target_valve_idxs:&Vec<usize>, current_idx:usize, ps: PS) -> u32 {
    if ps.time == 0 {
        return ps.total;
    }
    let mut options: Vec<u32> = vec![];
    //options: open current valve + travel to next valve, or wait it out until the end.
    //open valve options
    let this_valve = &vs.av[current_idx];
    let mut open_ps = ps;
    if this_valve.fr > 0 {
       open_ps = open_ps.open_valve(this_valve.fr);
    }
    for ni in target_valve_idxs {
        let dist_to_ni = sp[current_idx][*ni];
        if dist_to_ni <= open_ps.time {
            let new_targets:Vec<usize> = target_valve_idxs.iter().filter(|i|**i != *ni).map(|i|*i).collect();
            let new_ps = open_ps.next(dist_to_ni);
            let total_pressure = p1_dyn_r(vs, sp, &new_targets, *ni, new_ps);
            options.push(total_pressure);
        }
    }
    //just wait after opening current valve

    let wait_it_out_ps = open_ps.next(open_ps.time).total;
    options.push(wait_it_out_ps);

    options.sort();
    if options.len() > 3 {
        println!("options at time {}: {options:?}",open_ps.time);
    }
    return *options.last().expect("must be at least one option");
}

//Valve OJ has flow rate=0; tunnels lead to valves EW, IG
fn parse_valve(l:&String, idx:usize) -> Valve {
    let pts = util::string_split_multi(l, vec!["Valve "," has flow rate=","; "]);
    let key = pts[1].clone();
    let fr: u32 = pts[2].parse().expect("flow rate not a number");
    let next: Vec<Path> = if pts[3].starts_with("tunnels") {
        let pts2 = util::string_split_multi(&pts[3], vec![" lead to valves "]);
        pts2[1].split(", ")
        .map(|s|Path{dist:1, key:s.to_string()}).collect()
    } else {
        let pts2 = util::string_split_multi(&pts[3], vec![" leads to valve "]);
        vec![Path{dist:1, key:pts2[1].clone()}]
    };
    return Valve { idx, key, fr, next};
}


pub struct VS {
    av: Vec<Valve>,
    vm: HashMap<String,usize>
}

struct PS {
    time:u32,
    cr:u32,
    total:u32
}

impl PS {
    fn next(&self, dist:u32) -> PS {
        PS {
            time: self.time - dist,
            cr: self.cr,
            total: self.total + (self.cr * dist)
        }
    }

    fn open_valve(&self, fr:u32) -> PS {
        let prev_cr = self.cr;
        PS {
            time: self.time-1,
            cr: self.cr + fr,
            total: self.total + prev_cr
        }
    }
}

impl VS{
    fn find(&self, key: &String) -> &Valve {
        let i = self.vm.get(key).expect("could not find key");
        &self.av[*i]
    }
}

pub struct Path {
    dist: u32,
    key: String
}

impl fmt::Display for Path {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "({}, {})", self.key, self.dist)
    }
}

pub struct Valve {
    idx: usize,
    key: String,
    fr: u32,
    next: Vec<Path>
}