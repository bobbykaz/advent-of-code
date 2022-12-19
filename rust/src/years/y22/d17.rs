use std::collections::HashMap;

use crate::util;

pub fn run() {
    p1();
    p2();
}

pub fn p1() {
    let lines = util::read_file_into_lines("../input/y22/d17.txt");
    let lr_moves:Vec<char> = lines[0].chars().collect();
    let mut current_piece = Piece::WideLine;
    let board_size = 16000;
    let mut board = util::grid::new(7, board_size, '.');
    let mut max_height = 0;
    let mut lr_i = 0;
    let max: i64 = 2022;
    for r in 0..max {
        let mut piece_pos = piece_start_pos(max_height, &current_piece);
        let mut settled = false;
        if lr_i == 0 {
            println!("Starting cycle: Round {r}, max_height {max_height}, piece {current_piece:?}");
        }

        while !settled {
            //move sideways
            let next_move = lr_moves[lr_i];
            let move_offset = match next_move {
                '<' => -1,
                '>' => 1,
                _ => panic!("invalid sideways move direction {}", next_move)
            };

            let jet_p_pos: Vec<Pos> = piece_pos.iter()
                                    .map(|p| Pos {r:p.r, c: p.c+move_offset})
                                    .collect();

            //println!("Moving {} to {:?}", next_move, jet_p_pos);
            
            //move all pieces if they are in bound
            if piece_can_move(&board, &jet_p_pos, 0) {
                piece_pos = jet_p_pos;
            }

            lr_i += 1;
            lr_i %= lr_moves.len();

            //move down, if cant, settled
            let fall_p_pos: Vec<Pos> = piece_pos.iter()
                                    .map(|p| Pos {r:p.r-1, c: p.c})
                                    .collect();

            if piece_can_move(&board, &fall_p_pos, 0) {
                //println!("falling to {:?}", fall_p_pos);
                piece_pos = fall_p_pos;
            } else {
                //println!("Settled after just jet!");
                settled = true;
            }
        }

        //change pieces to settled piece type
        for p in piece_pos {
            if p.r + 1 > max_height {
                //println!("updating max height to {}", p.r);
                max_height = p.r + 1;
            }
            board.g[(p.r) as usize][p.c as usize] = '@';
        }

        current_piece = current_piece.next();
        //util::grid::print_grid_flip_y(&board, max_height as usize + 2);
        //println!("|======|");
    }

    println!("max height: {max_height}");
}


pub fn p2() {
    let lines = util::read_file_into_lines("../input/y22/d17.txt");
    let lr_moves:Vec<char> = lines[0].chars().collect();
    let mut current_piece = Piece::WideLine;
    let board_size = 640000;
    let swap_limit = 560000;
    let swap_amt: i64 = 480000;
    let mut board = util::grid::new(7, board_size, '.');
    let mut max_height = 0;
    let mut row_offset = 0;
    let mut lr_i = 0;
    let max: i64 = 1_000_000_000_000;
    let mut r: i64 = 0;
    let mut cycle_det_map: HashMap<String, (i64,i64)> = HashMap::new();
    let mut cycle_detected = false;
    while r < max {
        let mut piece_pos = piece_start_pos(max_height, &current_piece);
        let mut settled = false;

        // after it runs for a bit, cache all piece / lr_i / block_map entries until we see a repeat
        if r > 2022 && !cycle_detected {
            let cache_key = get_cache_key(&board, max_height, row_offset, &current_piece, lr_i);
             match cycle_det_map.get(&cache_key) {
                Some((prev_round, prev_max_height)) => {
                    cycle_detected = true;
                    println!("Found cycle from key {cache_key} ({} , {}) -> ({},{})", prev_round, prev_max_height, r, max_height);
                    let rdiff = r - prev_round;
                    let hdiff = max_height - prev_max_height;
                    while (r + rdiff) < max {
                        r += rdiff;
                        max_height += hdiff;
                        row_offset += hdiff;
                    }
                    piece_pos = piece_start_pos(max_height, &current_piece);
                    println!("fast forwarded to round {r}");
                },
                None => {
                    println!("caching {cache_key} -> ({},{})", r, max_height);
                    cycle_det_map.insert(cache_key, (r,max_height));
                }
             };
        }

        while !settled {
            //move sideways
            let next_move = lr_moves[lr_i];
            let move_offset = match next_move {
                '<' => -1,
                '>' => 1,
                _ => panic!("invalid sideways move direction {}", next_move)
            };

            let jet_p_pos: Vec<Pos> = piece_pos.iter()
                                    .map(|p| Pos {r:p.r, c: p.c+move_offset})
                                    .collect();

            //println!("Moving {} to {:?}", next_move, jet_p_pos);
            
            //move all pieces if they are in bound
            if piece_can_move(&board, &jet_p_pos, row_offset) {
                piece_pos = jet_p_pos;
            }

            lr_i += 1;
            lr_i %= lr_moves.len();

            //move down, if cant, settled
            let fall_p_pos: Vec<Pos> = piece_pos.iter()
                                    .map(|p| Pos {r:p.r-1, c: p.c})
                                    .collect();

            if piece_can_move(&board, &fall_p_pos, row_offset) {
                //println!("falling to {:?}", fall_p_pos);
                piece_pos = fall_p_pos;
            } else {
                //println!("Settled after just jet!");
                settled = true;
            }
        }

        //change pieces to settled piece type
        for p in piece_pos {
            if p.r + 1 > max_height {
                //println!("updating max height to {}", p.r);
                max_height = p.r + 1;
            }
            board.g[(p.r - row_offset) as usize][p.c as usize] = '@';
        }

        if (max_height - row_offset) > swap_limit {
            //can we just dump the bottom 6000 rows and hope thats good enough????
            //println!("ditching old rows at round {r}");
            for i in (0..swap_amt).rev() {
                board.g.swap_remove(i as usize);
            }

            for _ in 0..swap_amt {
                board.g.push(vec!['.','.','.','.','.','.','.'])
            }
            row_offset += swap_amt;
            //println!("done swapping");
        }

        current_piece = current_piece.next();
        r += 1;
        //util::grid::print_grid_flip_y(&board, max_height as usize + 2);
        //println!("|======|");
    }

    println!("max height: {max_height}");
}

fn get_cache_key(g: &util::grid::Grid<char>, max_height: i64, row_offset: i64, current_piece: &Piece, jet_index: usize) -> String {
    let mut key = match current_piece {
        Piece::WideLine => String::from("W-"),
        Piece::Cross => String::from("C-"),
        Piece::RevL => String::from("R-"),
        Piece::TallLine => String::from("T-"),
        Piece::Square =>String::from("S-")
    };

    key.push_str(&jet_index.to_string());
    key.push('-');

    let blockmap = get_filled_piece_map(g, max_height, row_offset);
    key.push_str(&blockmap);
    key
}

fn get_filled_piece_map(g: &util::grid::Grid<char>, max_height: i64, row_offset: i64) -> String {
    let mut rslt = String::new();
    for c in 0..g.width {
        let mut o = 0;
        let top = max_height - row_offset;
        while g.g[(top - o) as usize][c] == '.' {
            o+=1;
        }
        rslt.push_str(&o.to_string());
        rslt.push('-');
    }

    rslt
}

fn piece_can_move(g: &util::grid::Grid<char>, ps:&Vec<Pos>, row_offset: i64) -> bool {
    for p in ps {
        if !is_in_bounds(g, p, row_offset) {
            //println!("piece {} {} is not in bounds", p.r, p.c);
            return false;
        }
        if char_at(g, p, row_offset) != '.' {
            //println!("piece {} {} is not empty", p.r, p.c);
            return false;
        }
    }
    return true;
}

fn is_in_bounds(g: &util::grid::Grid<char>, p:&Pos, row_offset: i64) -> bool {
    (p.r - row_offset) >=0 && p.c >= 0 && p.c < g.width.try_into().unwrap()
}

fn char_at(g: &util::grid::Grid<char>, p:&Pos, row_offset: i64) -> char {
    if !is_in_bounds(g, p, row_offset) {panic!("pos not in bounds: ({},{})",p.r,p.c)}
    g.g[(p.r - row_offset) as usize][p.c as usize]
}

fn piece_start_pos(max_height: i64, piece_type: &Piece) -> Vec<Pos> {
    match piece_type{
        Piece::WideLine => {
            vec![
                Pos{r: max_height + 3,c: 2},
                Pos{r: max_height + 3,c: 3},
                Pos{r: max_height + 3,c: 4},
                Pos{r: max_height + 3,c: 5},
            ]
        },
        Piece::Cross => {
            vec![
                                        Pos{r: max_height + 5,c: 3},
            Pos{r: max_height + 4,c: 2},Pos{r: max_height + 4,c: 3},Pos{r: max_height + 4,c: 4},
                                        Pos{r: max_height + 3,c: 3}
            ]
        },
        Piece::RevL => {
            vec![
                                                                    Pos{r: max_height + 5,c: 4},
                                                                    Pos{r: max_height + 4,c: 4},
            Pos{r: max_height + 3,c: 2},Pos{r: max_height + 3,c: 3},Pos{r: max_height + 3,c: 4}                         
            ]
        },
        Piece::TallLine => {
            vec![
                Pos{r: max_height + 6,c: 2},
                Pos{r: max_height + 5,c: 2},
                Pos{r: max_height + 4,c: 2},
                Pos{r: max_height + 3,c: 2}
            ]
        },
        Piece::Square => {
            vec![
                Pos{r: max_height + 4,c: 2}, Pos{r: max_height + 4,c: 3},
                Pos{r: max_height + 3,c: 2}, Pos{r: max_height + 3,c: 3}
            ]
        }
    }
}

#[derive(Debug)]
struct Pos {
    r: i64,
    c: i64
}

#[derive(Debug)]
enum Piece {
    WideLine,
    Cross,
    RevL,
    TallLine,
    Square
}

impl Piece {
    fn next (&self) -> Piece {
        match *self {
            Self::WideLine => Self::Cross,
            Self::Cross => Self::RevL,
            Self::RevL => Self::TallLine,
            Self::TallLine => Self::Square,
            Self::Square => Self::WideLine
        }
    }
}

// rock order
// 4 wide
// cross
// reverse L 3x3
// 4 tall
// 2x2 sq, down left