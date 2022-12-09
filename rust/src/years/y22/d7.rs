use std::{collections::HashMap, cmp::Ordering};

use crate::util;

pub fn run() {
    pt1_structs();
}

fn pt1_structs() {
    let lines = util::read_file_into_lines("../input/y22/d7.txt");
    let mut dirs: Vec<Directory> = vec![];
    dirs.push(Directory {
        name: String::from("/"),
        dirs: vec![],
        files: vec![]
    });

    let mut files: Vec<File> = vec![];

    let mut path : Vec<usize> = vec![];
    path.push(0);

    for i in 1..lines.len() {
        let c_d_idx = path[path.len()-1];
        match parse_line(&lines[i]) {
            Output::LS => (),
            Output::NavUp => { path.pop(); },
            Output::Dir(d) => { 
                let d_idx = dirs.len();
                dirs.push(d);
                dirs[c_d_idx].dirs.push(d_idx); 
            },
            Output::File(f) => { 
                let f_idx = files.len();
                files.push(f);
                dirs[c_d_idx].files.push(f_idx); },
            Output::Nav(dirname) => {
                let sub_idx = find_subdir_idx(&dirs, &dirs[c_d_idx], &dirname);
                path.push(sub_idx);
                println!("Naving to {dirname}:{sub_idx}");
            }
        }
    }
    
    let mut sizes: Vec<i64> = vec![];
    //counting sizes...poorly
    for d in &dirs {
        sizes.push(dir_size(&dirs,&files, d));
    }

    sizes.sort();

    //sum sizes <= 100000

    let mut sum_low_sizes: i64 = 0;
    for s in sizes.iter() {
        println!("{s}");
        if *s <= 100000 {
            sum_low_sizes += s;
        }
    }

    println!("sum of small folders: {sum_low_sizes}");

    let total_disk = 70000000;
    let disk_needed = 30000000;
    let disk_in_use = total_disk - sizes[sizes.len()-1];
    let space_needed = disk_needed - disk_in_use;
    sizes.reverse(); 

    println!("need to free up {space_needed}");
    for s in sizes {
        if s >= space_needed {
            println!("could delete    {s}");
        }
    }

}

fn find_subdir_idx(all_dirs: &Vec<Directory>, this_dir: &Directory, name: &String) -> usize {
    for idx in &this_dir.dirs {
        if &(all_dirs[*idx].name) == name {
            return *idx
        }
    }
    let this_name = &this_dir.name;
    panic!("could not find {name} in {this_name}")
}

struct File {
    name: String,
    size: i64
}

struct Directory {
    name: String,
    dirs: Vec<usize>,
    files: Vec<usize>
}

enum Output {
    LS,
    NavUp,
    Nav(String),
    Dir(Directory),
    File(File)
}

fn parse_line(line: &String) -> Output {
    let pts: Vec<String> = line.split(" ").map(|x|x.to_string()).collect();
    match &pts[0] as &str {
        "$" => match &pts[1] as &str {
            "ls" => Output::LS,
            _ => match &pts[2] as &str {
                ".." => Output::NavUp,
                _ => Output::Nav(pts[2].clone())
            }
        },
        "dir" => Output::Dir(Directory{name:pts[1].clone(), dirs:vec![], files:vec![]}),
        _ => Output::File(File{name: pts[1].clone(), size: pts[0].parse().expect("failed to parse file")})
    }
}

fn dir_size(all_dirs: &Vec<Directory>, all_files: &Vec<File>, this_dir: &Directory) -> i64 {
    let mut sum: i64 = 0;
    for d in &this_dir.dirs {
        sum += dir_size(all_dirs,all_files,&all_dirs[*d]);
    }

    for f in &this_dir.files {
        let file = &all_files[*f];
        sum += file.size
    }
    //let n = &this_dir.name;
    //println!("dir {n} size: {sum}");
    sum
}


// got distracted trying to build this up with just strings
pub fn pt1_strings() {
    let lines = util::read_file_into_lines("../input/y22/d7.txt");
    let mut file_map: HashMap<String,i64> = HashMap::new();
    let mut all_files: Vec<String> = vec![];
    let mut path : Vec<String> = vec![];
    path.push(String::from("[root]"));

    all_files.push(pwd(&path));
    file_map.insert(pwd(&path), -1);

    //build map of full path -> filesize, -1 as a placeholder for uncalculated directories

    for i in 1..lines.len() {
        let pwd_str = pwd(&path);
        match parse_line(&lines[i]) {
            Output::LS => (),
            Output::NavUp => { path.pop(); },
            Output::Dir(d) => { 
                let dn = dir_path(&path, &d.name);
                file_map.insert(dn.clone(), 0);
                println!("adding dir {dn} from {pwd_str}");
                all_files.push(dn);
            },
            Output::File(f) => { 
                let f_n = file_path(&path, &f.name);
                file_map.insert(f_n.clone(), f.size);
                println!("adding file {f_n} from {pwd_str}");
                all_files.push(f_n);
            },
            Output::Nav(dirname) => {
                path.push(dirname.clone());
                println!("Naving from {pwd_str} to {dirname}");
            }
        }
    }

    // sort all paths by pathlength
    all_files.sort_by(compare_path_length);
    for f in all_files {
        let val = file_map.get(&f);
        println!("{f} -> {val:?}");
    }
    //find immediate children of each path
    //
}

pub fn is_file(name:&String) -> bool {
    match name.chars().last() {
        Some(c) if c == '/' => false,
        _ => true
    }
}

fn compare_path_length(a: &String, b: &String) -> Ordering {
    // Sort by length from short to long first.
    let a_dirs = a.matches("/").count();
    let b_dirs = b.matches("/").count();

    let length_test = a_dirs.cmp(&b_dirs);
    if length_test == Ordering::Equal {
        // If same length, sort in reverse alphabetical order.
        return a.cmp(b);
    }
    return length_test;
}

fn file_path(path_vec: &Vec<String>, file_name: &String) -> String {
    let mut path = pwd(path_vec);
    path.push_str(&file_name);
    path
}

fn dir_path(path_vec: &Vec<String>, dir_name: &String) -> String {
    let mut path = pwd(path_vec);
    path.push_str(&dir_name);
    path.push_str("/");
    path
}
fn pwd(path_vec: &Vec<String>) -> String {
    let mut path = String::new();
    for s in path_vec {
        path.push_str(s);
        path.push_str("/")
    }
    path
}