use std::collections::{HashSet, VecDeque};

use crate::util;

pub fn run() {
    //p1();
    p2();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d19.txt");
    let bps: Vec<Blueprint> = lines.iter().map(|l|parse_bp(l)).collect();
    let quality:u32 = bps.iter()
                    .map(|b| b.id * bp_max_geodes(b, 24))
                    .sum();

    println!("Quality: {quality}");
 }

 fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d19.txt");
    let bps: Vec<Blueprint> = lines.iter().take(3).map(|l|parse_bp(l)).collect();
    let quality:u32 = bps.iter()
                    .map(|b| bp_max_geodes(b, 32))
                    .sum();

    println!("Quality: {quality}");
 }

fn bp_max_geodes(bp: &Blueprint, time_limit: u32) -> u32 {
    let mut rslts: Vec<u32> = vec![];
    let mut queue: VecDeque<State> = VecDeque::new();
    let mut seen: HashSet<State> = HashSet::new();
    queue.push_back(State::new());
    seen.insert(State::new());
    println!("Processing bp: {}", bp.id);
    while queue.len() > 0 {
        let current = queue.pop_front().expect("should always work");
        //println!("current: {}", current.current_time);
        if current.current_time == time_limit {
            //println!("finished sim! {} -> {:?}", current.geodes, current);
            rslts.push(current.geodes);
        } else {
            if current.can_buy_geode_bot(bp) {
                //println!("buying geode bot at {}", current.current_time);
                let next = current.buy_geode_bot(bp);
                if !seen.contains(&next) {
                    seen.insert(State{..next});
                    queue.push_back(next);
                }
            } else {
                let mine = current.mine();
                if !seen.contains(&mine) {
                    seen.insert(State{..mine});
                    queue.push_back(mine);
                }
                if current.can_buy_ore_bot(bp) && current.or_b < 4 { // never need more than 4 ore in a turn
                    let next = current.buy_ore_bot(bp);
                    if !seen.contains(&next) {
                        seen.insert(State{..next});
                        queue.push_back(next);
                    }
                }

                if current.can_buy_clay_bot(bp) {
                    let next = current.buy_clay_bot(bp);
                    if !seen.contains(&next) {
                        seen.insert(State{..next});
                        queue.push_back(next);
                    }
                }

                if current.can_buy_obs_bot(bp) {
                    //println!("buying obs bot at {} - {:?}", current.current_time, current);
                    let next = current.buy_obs_bot(bp);
                    if !seen.contains(&next) {
                        seen.insert(State{..next});
                        queue.push_back(next);
                    }
                }
            }
        }
    }

    //println!("..results: {:?}", rslts);
    let max = rslts.into_iter().max().expect("need at least 1 value in rslts");
    println!("..max: {max}");
    return max;
}

#[derive(Eq, Hash, PartialEq, Debug)]
struct State {
    ore: u32,
    clay: u32,
    obsid: u32,
    geodes: u32,
    or_b: u32,
    c_b: u32,
    obs_b: u32,
    g_b: u32,
    current_time: u32
}

impl State {
    fn new() -> State {
        State { 
            ore: 0, 
            clay: 0, 
            obsid: 0,
            geodes: 0, 
            or_b: 1, 
            c_b: 0, 
            obs_b: 0, 
            g_b: 0, 
            current_time: 0 }
    }

    fn mine(&self) -> State {
        State { 
            ore: self.ore + self.or_b, 
            clay: self.clay + self.c_b, 
            obsid: self.obsid + self.obs_b, 
            geodes: self.geodes + self.g_b, 
            current_time: self.current_time + 1,
            ..*self 
        }
    }

    fn can_buy_ore_bot(&self, bp :&Blueprint) -> bool {
        self.ore >= bp.ore_bot
    }

    fn can_buy_clay_bot(&self, bp :&Blueprint) -> bool {
        self.ore >= bp.clay_bot
    }

    fn can_buy_obs_bot(&self, bp :&Blueprint) -> bool {
        self.ore >= bp.obs_bot.0 
        && self.clay >= bp.obs_bot.1
    }

    fn can_buy_geode_bot(&self, bp :&Blueprint) -> bool {
        self.ore >= bp.geode_bot.0 
        && self.obsid >= bp.geode_bot.1
    }

    fn buy_ore_bot(&self, bp :&Blueprint) -> State {
        if self.can_buy_ore_bot(bp) {
            let s1 = State {
                ore: self.ore - bp.ore_bot,
                ..*self
            };
            let s2 = s1.mine();
            return State {
                or_b: s2.or_b + 1,
                ..s2
            };
        } else {
            panic!("Cant actually buy ore bot!")
        }
    }

    fn buy_clay_bot(&self, bp :&Blueprint) -> State {
        if self.can_buy_clay_bot(bp) {
            let s1 = State {
                ore: self.ore - bp.clay_bot,
                ..*self
            };
            let s2 = s1.mine();
            return State {
                c_b: s2.c_b + 1,
                ..s2
            };
        } else {
            panic!("Cant actually buy clay bot!")
        }
    }

    fn buy_obs_bot(&self, bp :&Blueprint) -> State {
        if self.can_buy_obs_bot(bp) {
            let s1 = State {
                ore: self.ore - bp.obs_bot.0,
                clay: self.clay - bp.obs_bot.1,
                ..*self
            };
            let s2 = s1.mine();
            return State {
                obs_b: s2.obs_b + 1,
                ..s2
            };
        } else {
            panic!("Cant actually buy obs bot!")
        }
    }

    fn buy_geode_bot(&self, bp :&Blueprint) -> State {
        if self.can_buy_geode_bot(bp) {
            let s1 = State {
                ore: self.ore - bp.geode_bot.0,
                obsid: self.obsid - bp.geode_bot.1,
                ..*self
            };
            let s2 = s1.mine();
            return State {
                g_b: s2.g_b + 1,
                ..s2
            };
        } else {
            panic!("Cant actually buy obs bot!")
        }
    }

}

//Blueprint 18: Each ore robot costs 3 ore. 
//Each clay robot costs 4 ore. 
//Each obsidian robot costs 2 ore and 19 clay. 
//Each geode robot costs 2 ore and 12 obsidian.
fn parse_bp(l:&String) -> Blueprint {
    let splits = vec!["Blueprint ",
                    ": Each ore robot costs ",
                    " ore. Each clay robot costs ",
                    " ore. Each obsidian robot costs ",
                    " ore and ",
                    " clay. Each geode robot costs ",
                    " ore and ",
                    " obsidian."];
    let pts = util::string_split_multi(l, splits);
    Blueprint { 
        id: pts[1].parse().expect("couldnt parse id"), 
        ore_bot: pts[2].parse().expect("couldnt parse ore cost"), 
        clay_bot: pts[3].parse().expect("couldnt parse clay cost"), 
        obs_bot: (pts[4].parse().expect("couldnt parse obscost 1"),pts[5].parse().expect("couldnt parse obscost 1")), 
        geode_bot: (pts[6].parse().expect("couldnt parse geode cost 1"),pts[7].parse().expect("couldnt parse geode cost 1")) 
    }
}

#[derive(Debug)]
struct Blueprint {
    id: u32,
    ore_bot: u32,
    clay_bot: u32,
    obs_bot: (u32,u32),
    geode_bot: (u32,u32)
}