use std::{vec, cmp::Ordering};

use crate::util;

pub fn run() {
    let chunks = util::read_file_into_chunks("../input/y22/d13.txt", "\n\n");
    let mut in_order: Vec<usize> = vec![]; 
    let mut all_nodes: Vec<Node> = vec![];

    for i in 0..chunks.len(){
        println!("{}", chunks[i]);
        let lines = util::read_string_into_lines(&chunks[i]);
        let n1 = parse_line(&lines[0]);
        let n2 = parse_line(&lines[1]);
        if n1.less_than(&n2) == Ordering::Less {
            in_order.push(i+1);
        }
        all_nodes.push(n1);
        all_nodes.push(n2);
    }

    println!("{in_order:?}");
    let total:usize = in_order.iter().sum();
    println!("total: {total}");

    all_nodes.push(parse_line(&String::from("[[2]]")));
    all_nodes.push(parse_line(&String::from("[[6]]")));

    all_nodes.sort();
    let d1 = parse_line(&String::from("[[2]]"));
    let d2 = parse_line(&String::from("[[6]]"));
    let mut d1i = 0;
    let mut d2i = 0;

    for i in 0..all_nodes.len() {
        if all_nodes[i] == d1 {
            println!("Found div 1 at {i}");
            d1i = i + 1;
        }

        if all_nodes[i] == d2 {
            println!("Found div 2 at {i}");
            d2i = i + 1;
        }
    }

    println!("Product: {}", (d1i * d2i));
}

fn parse_line(l: &String) -> Node {
    let chars: Vec<char> = l.chars().collect();
    let (rslt,_) = parse_line_r(&chars, 0);
    rslt
}

fn parse_line_r(l: &Vec<char>, s:usize) -> (Node, usize) {
    let mut this_node = Node{
        list: vec![],
        value: None
    };
    
    let mut i: usize = s;
    
    while i < l.len() {
        match l[i] {
            '[' => {
                let (sub_node, end_idx) = parse_line_r(l, i+1);
                this_node.list.push(sub_node);
                i = end_idx;
            },
            ']' => {return (this_node, i + 1);},
            ',' =>{ i += 1;},
            ch if ch.is_digit(10) => {
                let mut curr_string = String::new();
                curr_string.push(ch);
                i += 1;
                while l[i].is_digit(10) {
                    curr_string.push(ch);
                    i += 1;
                }
                let v_node = Node {
                    list: vec![],
                    value: Some(curr_string.parse().expect("couldnt parse value string"))
                };
                this_node.list.push(v_node);
            },
            _ => panic!("unexpected char")
        }
    }

    (this_node, i)

}
#[derive(Eq)]
struct Node {
    list: Vec<Node>,
    value: Option<i32>
}


impl Ord for Node{
    fn cmp(&self, other: &Self) -> Ordering {
        self.less_than(other)
    }
}

impl PartialOrd for Node {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl PartialEq for Node {
    fn eq(&self, other: &Self) -> bool {
        self.cmp(other) == Ordering::Equal
    }
}

impl Node {
    pub fn less_than(&self, other: &Node) -> Ordering {
        match (self.value, other.value) {
            (Some(a), Some(b)) => {
                let rslt = a.cmp(&b);
                //println!("comparing {a} to {b}: {rslt:?}");
                return rslt;
            },
            (None, Some(b)) => {
                //println!("turning B into list {b}");
                let b_list = Node::value_to_list(b);
                return Node::list_less_than(&self.list, &b_list.list);
            },
            (Some(a), None) => {
                //println!("turning a into list {a}");
                let a_list = Node::value_to_list(a);
                return Node::list_less_than(&a_list.list, &other.list);
            },
            (None, None) => {
                //println!("comparing lists");
                return Node::list_less_than(&self.list, &other.list);
            }
        }
    }

    fn value_to_list(v: i32) -> Node {
        Node {
            list: vec![Node{list:vec![], value: Some(v)}],
            value: None
        }
    }

    fn list_less_than(a:&Vec<Node>,b: &Vec<Node>) -> Ordering {
        let a_is_shorter = a.len() < b.len();
        for i in 0..a.len() {
            if i >= b.len() {return Ordering::Greater;}

            match a[i].less_than(&b[i]) {
                Ordering::Less => {return Ordering::Less;},
                Ordering::Equal => {/*println!("Equal Ordering found, continueing");*/},
                Ordering::Greater => {return Ordering::Greater;}
            }
        }
        //println!("list a ran out - does a have fewer items than b? {a_is_shorter}");
        match a_is_shorter {
            true => Ordering::Less,
            false => Ordering::Equal
        }
    }
}