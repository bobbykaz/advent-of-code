use std::cmp::{min, max};

use crate::util;

pub fn run() {
    p2();
}

fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d15s.txt");
    let sens: Vec<Sensor> = lines.into_iter().map(|x|parse_line(&x)).collect();

    let minX = sens.iter().map(|s|min(s.pos.x, s.cb.x)).min().expect("must have min value")-10000000;
    let maxX = sens.iter().map(|s|max(s.pos.x, s.cb.x)).max().expect("must have max value")+10000000;
    let mut impossible:u64 = 0;
    for x in minX..(maxX+1) {
        let this = Pos{x,y:2000000}; //2000000 or 10
        let mut is_beacon = false;
        let mut is_sensor = false;
        let mut is_in_empty_rad = false;
        for s in sens.iter() {
            if this.dist(&s.pos) <= s.empty_rad {
                is_in_empty_rad = true;
            }

            if this.x == s.pos.x && this.y == s.pos.y {
                is_sensor = true;
            } 

            if this.x == s.cb.x && this.y == s.cb.y {
                is_beacon = true;
            } 
        }

        if is_in_empty_rad && !is_beacon && !is_sensor {
            impossible += 1;
        }
    }
    println!("total impossible: {impossible}");
}

fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d15.txt");
    let bound: i64 = 4000001; //4000001 or 21
    let sens: Vec<Sensor> = lines.into_iter().map(|x|parse_line(&x)).collect();
    for s in sens.iter() {
        //left boundary to top
        let lbx = s.pos.x - s.empty_rad - 1;
        for i in 0..(s.empty_rad+1) {
            let this = Pos{
                x: lbx + i,
                y: s.pos.y + i
            };
            if point_is_beacon(&this, &sens, bound) {
                println!("Freq : {}",(this.x * 4000000 + this.y))
            }
        }
        // top to right
        let uby = s.pos.y + s.empty_rad + 1;
        for i in 0..(s.empty_rad+1) {
            let this = Pos{
                x: s.pos.x + i,
                y: uby - i
            };
            if point_is_beacon(&this, &sens, bound) {
                println!("Freq : {}",(this.x * 4000000 + this.y))
            }
        }
        // right to bottom
        let ubx = s.pos.x + s.empty_rad + 1;
        for i in 0..(s.empty_rad+1) {
            let this = Pos{
                x: ubx - i,
                y: s.pos.y - i
            };
            if point_is_beacon(&this, &sens, bound) {
                println!("Freq : {}",(this.x * 4000000 + this.y))
            }
        }
        // bottom to left
        let lby = s.pos.y - s.empty_rad - 1;
        for i in 0..(s.empty_rad+1) {
            let this = Pos{
                x: s.pos.x - i,
                y: lby + i
            };
            if point_is_beacon(&this, &sens, bound) {
                println!("Freq : {}",(this.x * 4000000 + this.y))
            }
        }
        println!("checked {s:?} {lbx}-{ubx}, {lby}-{uby}");
    }
}

fn point_is_beacon(this: &Pos, sensors: &Vec<Sensor>, bound: i64) -> bool {
    if  this.x < 0 || this.x > bound || this.y < 0 || this.y >bound {
        return false;
    }
    let mut is_beacon = false;
    let mut is_sensor = false;
    let mut is_in_empty_rad = false;
    for s2 in sensors.iter() {
        if this.dist(&s2.pos) <= s2.empty_rad {
            is_in_empty_rad = true;
        }

        if this.x == s2.pos.x && this.y == s2.pos.y {
            is_sensor = true;
        } 

        if this.x == s2.cb.x && this.y == s2.cb.y {
            is_beacon = true;
        } 
    }

    if !is_in_empty_rad && !is_beacon && !is_sensor {
        println!("beacon must be at: {this:?}");
        return true;
    }
    return false;
}
//Sensor at x=2483411, y=3902983: closest beacon is at x=2289579, y=3633785
fn parse_line(l: &String) -> Sensor {
    let pts = util::string_split_multi(l, vec!["Sensor at x=",", y=",": closest beacon is at x=",", y="]);
    println!("{pts:?}");
    let b = Pos {
        x: pts[3].parse().expect("fail b x"),
        y: pts[4].parse().expect("fail b y,"),
    };
    let sp = Pos{
        x: pts[1].parse().expect("fail s x"),
        y: pts[2].parse().expect("fail s y,"),
    };
    let d = sp.dist(&b);
    let s = Sensor {
        pos: sp,
        cb: b,
        empty_rad: d
    };

    println!("s: {s:?}");

    s
}

#[derive(Debug)]
struct Sensor {
    pos: Pos,
    cb: Pos,
    empty_rad:i64
}

#[derive(Debug)]
struct Pos {
    x:i64,
    y:i64
}

impl Pos {
    fn dist(&self, other: &Pos) -> i64 {
        let xd = self.x - other.x;
        let yd = self.y - other.y;
        xd.abs() + yd.abs()
    }
}