use crate::util;

pub fn run() {
    p1();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d25.txt");
    let total: i64 = lines.iter().map(|l|parse_snafu(l)).sum();
    println!("total: {total}");

    let snafu_total = dec_to_snafu(total);
    println!("snafu: {}", snafu_total);
    let dc = parse_snafu(&snafu_total);
    println!("double check {}", dc);
 }

 fn parse_snafu(l:&String) -> i64 {
    let chars = l.chars().rev();
    let mut i = 0;
    let mut sum = 0;
    for c in chars {
        let d:i64 = match c {
            '=' => -2,
            '-' =>-1,
            '0' => 0,
            '1' => 1,
            '2' => 2,
            _ => unreachable!()
        };
        sum += d * (5_i64.pow(i));
        i+=1
    }

    println!("{l} -> {sum}");
    sum
 }

 fn dec_to_snafu(n: i64) -> String {
    let mut current = n;
    let mut rslt = String::new();
	while current > 0 {
        let base = current % 5;
        match base {
            0 => {
                rslt.push('0');
                current /= 5;
            },
            1 => {
                rslt.push('1');
                current /= 5;
            },
            2 => {
                rslt.push('2');
                current /= 5;
            },
            3 => {
                rslt.push('=');
                current += 2;
                current /= 5;
            },
            4 => {
                rslt.push('-');
                current += 1;
                current /= 5;
            },
            _ => unreachable!()
        } ;
    }

    rslt.chars().rev().collect()
 }

    //     29694520452605
    //18 -> 3814697265625
    //19 -> 19073486328125
    //20 -> 95367431640625
    //5^^12343003334213440410