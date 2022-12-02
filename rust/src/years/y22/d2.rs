use crate::utilities::read_file_into_lines;

pub fn run() {
    let lines = read_file_into_lines("../input/y22/d2.txt");
    
    let p1 = pt1(&lines);
    println!("Part 1: {p1}");

    let p2 = pt2(&lines);
    println!("Part 2: {p2}");
}

pub fn pt1(lines: &Vec<String>) -> i32 {
    let mut score: i32 = 0;

    for i in 0..lines.len() {
        let moves: Vec<String> = lines[i].split(" ")
        .map(|x|x.to_string())
        .collect();
        let opp = &moves[0];
        let me = &moves[1];
        let rslt = battle(opp, me);
        score += rslt;
    }

    score
}

pub fn pt2(lines: &Vec<String>) -> i32 {
    let mut score: i32 = 0;

    for i in 0..lines.len() {
        let moves: Vec<String> = lines[i].split(" ")
        .map(|x|x.to_string())
        .collect();
        let opp = &moves[0];
        let me = &moves[1];
        let rslt = battle_2(opp, me);
        score += rslt;
    }

    score
}

fn battle (opp: &String, me: &String) -> i32 {
    let play_score = match me as &str {
        "X" => 1,
        "Y" => 2,
        "Z" => 3,
        _ => panic!("Bad move")
    };

    let win_score = match opp as &str {
        "A" =>
            match me as &str {
                "X" => 3,
                "Y" => 6,
                "Z" => 0,
                _ => panic!("Bad move")
            }
        "B" =>
            match me as &str {
                "X" => 0,
                "Y" => 3,
                "Z" => 6,
                _ => panic!("Bad move")
            }
        "C" =>
            match me as &str {
                "X" => 6,
                "Y" => 0,
                "Z" => 3,
                _ => panic!("Bad move")
            }
        _ => panic!("Bad opp move")
    };

    play_score + win_score
}

fn battle_2 (opp: &String, me: &String) -> i32 {
    let win_score = match me as &str {
        "X" => 0,
        "Y" => 3,
        "Z" => 6,
        _ => panic!("Bad move")
    };

    let play_score = match opp as &str {
        "A" =>
            match me as &str {
                "X" => 3,
                "Y" => 1,
                "Z" => 2,
                _ => panic!("Bad move")
            }
        "B" =>
            match me as &str {
                "X" => 1,
                "Y" => 2,
                "Z" => 3,
                _ => panic!("Bad move")
            }
        "C" =>
            match me as &str {
                "X" => 2,
                "Y" => 3,
                "Z" => 1,
                _ => panic!("Bad move")
            }
        _ => panic!("Bad opp move")
    };

    play_score + win_score
}
