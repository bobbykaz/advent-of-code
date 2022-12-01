use crate::utilities::read_file_into_lines;

pub fn run() {
    let lines = read_file_into_lines("../input/y22/d1.txt");
    
    pt1(&lines);
}

pub fn pt1(lines: &Vec<String>) {
    let mut max_elf = 0;
    let mut cur_elf = 0;

    let mut all_elves: Vec<i32> = Vec::new();

    for i in 0..lines.len() {
        if lines[i] == String::from("") {
            println!("Completed elf: {cur_elf}");
            all_elves.push(cur_elf);
            if cur_elf > max_elf {
                max_elf = cur_elf;
                println!("new max! : {max_elf}");
            }
            cur_elf = 0;
        } else {
            let cal: i32 = lines[i].parse().expect("wasnt a number: {lines[i]}!");
            println!("   adding {cal} to {cur_elf}");
            cur_elf += cal;
        }
    }

    println!("Max Elf: {max_elf}");
    all_elves.sort();
    all_elves.reverse();
    let total_iter: i32 = all_elves.iter().take(3).sum();
    println!("total: {total_iter}");
}
