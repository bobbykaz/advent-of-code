use std::{fs};

pub mod grid;

fn read_file_to_string(filepath: &str) -> String {
    let contents = fs::read_to_string(filepath)
        .expect("Could not read file {filepath}");

    contents.replace("\r\n", "\n")
}

pub fn read_file_into_lines(filepath: &str) -> Vec<String> {
    read_file_into_chunks(filepath, "\n")
}

pub fn read_string_into_lines(text: &String) -> Vec<String> {
    let mut vec_rslt: Vec<String> = text.split("\n")
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

pub fn read_file_into_chunks(filepath: &str, chunk_separator: &str) -> Vec<String> {
    let trimmed_contents = read_file_to_string(filepath);

    let mut vec_rslt: Vec<String> = trimmed_contents.split(chunk_separator)
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

pub fn string_to_ints(l: &String, delim: &str) -> Vec<i32> {
    l.split(delim).into_iter()
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