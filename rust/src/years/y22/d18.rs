use crate::util;

pub fn run() {
    let lines = util::read_file_into_lines("../input/y22/d18.txt");
    let mut field:Vec<Vec<Vec<u16>>> = vec![];
    for i in 0..23 {
        field.push(vec![]);
        for j in 0..23 {
            field[i].push(vec![]);
            for _ in 0..23 {
                field[i][j].push(0)
            }
        }
    }
    for l in lines {
        let (x,y,z) = parse_line(&l);
        field[x][y][z] = 1;
    }

    let mut sa = 0;
    for i in 0..23 {
        for j in 0..23 {
            for k in 0..23 {
                sa += surface_area(&field, i,j,k, 0);
            }
        }
    }

    println!("Surface Area included trapped air: {sa}");

    // mark outer shells as touching air (2 if currently 0), then flood inward

    //xz plane
    for x in 0..23 {
        for z in 0..23 {
            if field[x][0][z] == 0 { field[x][0][z] = 2;}
            if field[x][22][z] == 0 { field[x][22][z] = 2;}
        }
    }
    //xy plane
    for x in 0..23 {
        for y in 0..23 {
            if field[x][y][0] == 0 { field[x][y][0] = 2;}
            if field[x][y][22] == 0 { field[x][y][22] = 2;}
        }
    }
    //yz plane
    for y in 0..23 {
        for z in 0..23 {
            if field[0][y][z] == 0 { field[0][y][z] = 2;}
            if field[22][y][z] == 0 { field[22][y][z] = 2;}
        }
    }

    //successive shells inward or flood fills is more efficient, but lets just brute force the whole thing
    let mut any_changes = true;
    while any_changes {
        any_changes = false;
        for i in 1..22 {
            for j in 1..22 {
                for k in 1..22 {
                    if field[i][j][k] == 0 
                    && cell_has_neighbor(&field, i, j, k, 2) {
                        field[i][j][k] = 2;
                        any_changes = true;
                    }
                }
            }
        }
    }

    let mut sa_2 = 0;
    for i in 0..23 {
        for j in 0..23 {
            for k in 0..23 {
                sa_2 += surface_area(&field, i,j,k, 2);
            }
        }
    }

    println!("Surface Area minus trapped air: {sa_2}");
}

fn cell_has_neighbor(field: &Vec<Vec<Vec<u16>>>, x: usize, y: usize, z: usize, target:u16) -> bool {
    if field[x+1][y][z] == target { return true ;}
    if x > 0 && field[x-1][y][z] == target { return true ; }
    if field[x][y+1][z] == target { return true ; }
    if y > 0 && field[x][y-1][z] == target { return true ; }
    if field[x][y][z+1] == target { return true ; }
    if z > 0 && field[x][y][z-1] == target { return true ; }

    return false
}

fn surface_area(field: &Vec<Vec<Vec<u16>>>, x: usize, y: usize, z: usize, target:u16) -> u32 {
    if field[x][y][z] == 1 {
        let mut count = 0;
        if field[x+1][y][z] == target { count += 1; }
        if  x > 0 && field[x-1][y][z] == target { count += 1; }
        if x == 0 { count+=1 }
        if field[x][y+1][z] == target { count += 1; }
        if y > 0 && field[x][y-1][z] == target { count += 1; }
        if y == 0 { count+=1 }
        if field[x][y][z+1] == target { count += 1; }
        if z > 0 && field[x][y][z-1] == target { count += 1; }
        if z == 0 { count+=1 }
        count
    } else {
        0
    }
}

fn parse_line(l: &String) -> (usize,usize,usize) {
    let p = util::string_to_ints(l,",");
    (p[0] as usize, p[1] as usize,p[2]as usize)
}
   