use std::usize;

use crate::util::{self, strings_to_ints};

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d20.txt");
    let nums = strings_to_ints(&lines);
    let mut nums_with_i: Vec<(i64,usize)> = Vec::new();
    let mut zero_index = 0;
    for i in 0..nums.len() {
        if nums[i] == 0 {
            zero_index = i;
        }
        nums_with_i.push((nums[i] as i64,i));
    }
    let og_nums = strings_to_ints(&lines);
    //println!("Start: {:?}", nums_with_i);
    for n in 0..og_nums.len() {
        let idx = index_of(n, &nums_with_i);
        nums_with_i = mix_2(nums_with_i, idx);
        //println!("{:?}", nums_with_i);
    }

    let idx = index_of(zero_index, &nums_with_i);

    let i1 = (idx + 1000) % nums.len();
    let i2 = (idx + 2000) % nums.len();
    let i3 = (idx + 3000) % nums.len();

    let sum = nums_with_i[i1].0 + nums_with_i[i2].0 + nums_with_i[i3].0;

    println!("found {} {} {}",nums_with_i[i1].0, nums_with_i[i2].0, nums_with_i[i3].0);
    println!("{sum}");

}

pub fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d20.txt");
    let nums = strings_to_ints(&lines);
    let mut nums_with_i: Vec<(i64,usize)> = Vec::new();
    let mut zero_index = 0;
    for i in 0..nums.len() {
        if nums[i] == 0 {
            zero_index = i;
        }
        nums_with_i.push(((nums[i] as i64) * 811589153,i));
    }
    let og_nums = strings_to_ints(&lines);
    //println!("Start: {:?}", nums);
    for n in 0..og_nums.len() {
        let idx = index_of(n, &nums_with_i);
        nums_with_i = mix(nums_with_i, idx);
        println!("mixed {n}");
        //println!("{:?}", nums);
    }

    let idx = index_of(zero_index, &nums_with_i);

    let i1 = (idx + 1000) % nums.len();
    let i2 = (idx + 2000) % nums.len();
    let i3 = (idx + 3000) % nums.len();

    let sum = nums_with_i[i1].0 + nums_with_i[i2].0 + nums_with_i[i3].0;

    println!("found {} {} {}",nums_with_i[i1].0, nums_with_i[i2].0, nums_with_i[i3].0);
    println!("{sum}");

}

fn mix(mut v: Vec<(i64,usize)>, idx: usize) -> Vec<(i64,usize)> {
    let start = v[idx].0;
    if start < 0 {
        let mut from = idx as i32;
        let mut to = from - 1;
        if to < 0 {
            to = v.len() as i32 - 1;
        }
        for _ in 0..start.abs() {
            let tmp = v[to as usize];
            v[to as usize] = v[from as usize];
            v[from as usize] = tmp;
            from -= 1;
            if from < 0 {
                from = v.len() as i32 - 1;
            }
            to -= 1;
            if to < 0 {
                to = v.len() as i32 - 1;
            }
        }
        println!("removed {} at {}, inserted to {}", start, idx, to);
    } else if start > 0 {
        for i in 0..(start as usize) {
            let from = (idx + i) % v.len();
            let to = (idx + i + 1) % v.len();
            let tmp = v[to];
            v[to] = v[from];
            v[from] = tmp;
        }
        println!("removed {} at {}, inserted to {}", start, idx, (idx as i64 + start) % v.len() as i64);
    }

    return v;
}

// [0, 1, 2, -5, 4, 5, 6, 7 ,8, 9] =>
//        ^--- 6 goes here (2 moves left) - insert at index 3
//                           ^----- -5 swaps to here (8 moves right?) insert at index 8?
fn swap_test(mut nums:Vec<i32>, i: usize) -> Vec<i32> {
    let target: i32 = nums[i];
    let amt =  (i as i32 + target).rem_euclid(nums.len() as i32) as usize;
    println!("========");
    println!("{nums:?} ...");
    nums.remove(i);
    nums.insert(amt, target);
    println!("swapped {i} with {amt}");
    println!("...{nums:?}");
    return nums;
}

fn mix_2(mut v: Vec<(i64,usize)>, idx: usize) -> Vec<(i64,usize)> {
    let start = v[idx].0;
    if start < 0 {
        let delta = (idx as i64 + start).abs() % v.len() as i64; 
        let mut to_idx = v.len() - (delta as usize) - 1;
        let item = v.remove(idx);
        //println!("removed {} at {}, inserted to {}", item.0, idx, to_idx);
        v.insert(to_idx, item);
    } else if start > 0 {
        let to_idx = (idx as i64 + start) % v.len() as i64; 
        let item = v.remove(idx);
        //println!("removed {} at {}, inserted to {}", item.0, idx, to_idx);
        v.insert(to_idx as usize, item);
    }

    return v;
}

fn index_of(target_idx:usize, v:&Vec<(i64,usize)>) -> usize {
    for i in 0..v.len() {
        if v[i].1 == target_idx {
            return i;
        }
    }

    panic!("target {} not in list!", target_idx);
}
/* 
#[cfg(test)]
mod tests {
    #[test]
    fn larger_can_hold_smaller() {
        let larger = Rectangle {
            width: 8,
            height: 7,
        };
        let smaller = Rectangle {
            width: 5,
            height: 1,
        };

        assert!(larger.can_hold(&smaller));
    }
}*/