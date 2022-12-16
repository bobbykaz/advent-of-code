use std::collections::HashMap;

use crate::util;

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d16.txt");
    let all_valves: Vec<Valve> = lines.iter().map(|l|parse_valve(l)).collect();
    let mut vmap: HashMap<String,usize> = HashMap::new();
    for i in 0..all_valves.len() {
        vmap.insert(all_valves[i].key.clone(), i);
    }

    let vs = VS { av: all_valves, vm: vmap};
    greedy(&vs);

}

fn greedy(vs: &VS) {
    let start = vs.find(&String::from("AA"));
    let result = greedy_r(vs, start, &PS{time:30, cr:0, total:0}, &String::new());
    println!("Result: {result}");
}

fn greedy_r(vs: &VS, current:&Valve, ps: &PS, seen: &String) -> u32 {
    if ps.time == 0 {
        return ps.total;
    }
    let open_this_valve = current.fr > 0 && !seen.contains(&current.key);
    let mut options: Vec<u32> = vec![];
    if open_this_valve {
        //println!("checking opening {} at {}", current.key, ps.time);
        let mut next_seen: String = seen.clone();
        next_seen.push('-');
        next_seen.push_str(&current.key);
        let interim_ps = ps.open_valve(current.fr);
        if interim_ps.time == 0 {
            return interim_ps.total;
        }
        let travel_ps = &interim_ps.next();
        for next in vs.next(current) {
            let pos = greedy_r(vs, next, travel_ps, &next_seen);
            options.push(pos);
        }
    }
    let travel_ps = ps.next();
    for next in vs.next(current) {
        let pos = greedy_r(vs, next, &travel_ps, &seen);
        options.push(pos);
    }

    options.sort();
    return *options.last().expect("must be at least one option");
}



//Valve OJ has flow rate=0; tunnels lead to valves EW, IG
fn parse_valve(l:&String) -> Valve {
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
    return Valve { key, fr, next};
}


struct VS {
    av: Vec<Valve>,
    vm: HashMap<String,usize>
}

struct PS {
    time:u32,
    cr:u32,
    total:u32
}

impl PS {
    fn next(&self) -> PS {
        PS {
            time: self.time-1,
            cr: self.cr,
            total: self.total + self.cr
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
    fn next(&self, v: &Valve) -> Vec<&Valve> {
        v.next.iter()
        .map(|k|self.find(&k.key)).
        collect()
    }
    fn find(&self, key: &String) -> &Valve {
        let i = self.vm.get(key).expect("could not find key");
        &self.av[*i]
    }
}

struct Path {
    dist: u32,
    key: String
}

struct Valve {
    key: String,
    fr: u32,
    next: Vec<Path>
}