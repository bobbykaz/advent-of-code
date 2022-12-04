use std::{fs};

pub fn read_file_into_lines(filepath: &str) -> Vec<String> {
    println!("In file {}", filepath);

    let contents = fs::read_to_string(filepath)
        .expect("Could not read file {filepath}");

    let trimmed_contents = contents.replace("\r\n", "\n");

    let mut vec_rslt: Vec<String> = trimmed_contents.split("\n")
    .map(|x|x.to_string())
    .collect();

    match vec_rslt.last() {
        Some(str) if str == "" => {
            vec_rslt.remove(vec_rslt.len() -1);
            vec_rslt
        }
        _ => 
            vec_rslt
    }
}

pub fn strings_to_ints(lines: &Vec<String>) -> Vec<i32> {
    lines.into_iter()
    .map(|x| x.parse().expect("failed to parse {x} into i32"))
    .collect()
}

pub fn string_split_multi(line: &String, splits: Vec<&str>) -> Vec<String> {
    let mut rslt: Vec<String> = Vec::new();
    let mut remaining = line.clone();
    for spl in splits {
        match remaining.split_once(spl) {
         Some((a,b)) => {
            rslt.push(a.into());
            remaining = b.into();
         },
         None => 
            panic!("failed to split {remaining} on {spl}")
        };
    }
    rslt.push(remaining);
    rslt
}