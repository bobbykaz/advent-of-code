use crate::util;

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d17s.txt");
    let lr_moves:Vec<char> = lines[0].chars().collect();
    let mut current_piece = Piece::WideLine;
    let mut board = util::grid::new(7, 2022*4, '.');
    let mut max_height = 0;
    let mut lr_i = 0;
    for c in 0..3 {
        let mut piece_pos = piece_start_pos(max_height, &current_piece);
        let mut settled = false;
        println!("starting piece {c}");

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
            
            //move all pieces if they are in bound
            if piece_can_move(&board, &jet_p_pos) {
                piece_pos = jet_p_pos;
            }

            lr_i += 1;
            lr_i %= lr_moves.len();

            //move down, if cant, settled
            let fall_p_pos: Vec<Pos> = piece_pos.iter()
                                    .map(|p| Pos {r:p.r-1, c: p.c})
                                    .collect();

            if piece_can_move(&board, &fall_p_pos) {
                piece_pos = fall_p_pos;
            } else {
                settled = true;
            }
        }

        //change pieces to settled piece type
        for p in piece_pos {
            if p.r > max_height {
                max_height = p.r;
            }
            board.g[p.r as usize][p.c as usize] = '@';
        }

        current_piece = current_piece.next();
        util::grid::print_grid_flip_y(&board, max_height as usize + 2);
        println!("|======|");
    }

    println!("max height: {max_height}");
}

fn piece_can_move(g: &util::grid::Grid<char>, ps:&Vec<Pos>) -> bool {
    for p in ps {
        if !is_in_bounds(g, p) {
            println!("piece {} {} is not in bounds", p.r, p.c);
            return false;
        }
        if char_at(g, p) != '.' {
            println!("piece {} {} is not empty", p.r, p.c);
            return false;
        }
    }
    return true;
}

fn is_in_bounds(g: &util::grid::Grid<char>, p:&Pos) -> bool {
    p.r >=0 && p.c >= 0 && p.c < g.width.try_into().unwrap()
}

fn char_at(g: &util::grid::Grid<char>, p:&Pos) -> char {
    if !is_in_bounds(g, p) {panic!("pos not in bounds: ({},{})",p.r,p.c)}
    g.g[p.r as usize][p.c as usize]
}

fn piece_start_pos(max_height: i32, piece_type: &Piece) -> Vec<Pos> {
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

struct Pos {
    r: i32,
    c: i32
}

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