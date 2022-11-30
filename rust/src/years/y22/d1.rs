use crate::utilities::read_file_into_lines;
use crate::utilities::convert_lines_to_ints;

pub fn run() {
    let lines = read_file_into_lines("../input/y22/d1.txt");
    let nums = convert_lines_to_ints(&lines);
    pt1(&nums);
    pt2(&nums);
}

pub fn pt1(nums: &Vec<i32>) {
    let mut count = 0;

    for i in 1..nums.len() {
        if nums[i] > nums[i-1] {
            count+=1;
        }
    }

    println!("Number of increasing values: {count}")
}

pub fn pt2(nums: &Vec<i32>) {
    let mut sums: Vec<i32> = Vec::new();
    
    for i in 2..nums.len() {
        let sum = nums[i] + nums[i-1] + nums[i-2];
        sums.push(sum)
    }
    
    println!("Part 2:");
    pt1(&sums);
}