pub fn run() {
    p1();
}

pub fn p1() {
    let mut monkeys = get_monkeys();
    let mut monk_interactions = vec![0,0,0,0,0,0,0,0];

    for r in 0..1000 {
        println!("starting round {r}");
        for m in 0..monkeys.len() {
            for _ in 0..monkeys[m].items.len() {
                let item = monkeys[m].items.remove(0);
                let worry = match monkeys[m].op {
                    MnkOp::Add(x) => item + x,
                    MnkOp::Mul(x) => item * x,
                    //MnkOp::Div(x) => item / x,
                    MnkOp::Sq => item * item
                };

                //worry /= 3;

                if worry % monkeys[m].test == 0 {
                    let target = monkeys[m].true_target;
                    monkeys[target].items.push(worry);
                } else {
                    let target = monkeys[m].false_target;
                    monkeys[target].items.push(worry);
                }
                monk_interactions[m] += 1;
            }
        }
    }

    monk_interactions.sort();
    println!("total interactions: {monk_interactions:?}");
    let mb = monk_interactions[0] * monk_interactions[1];
    println!("monkey business: {mb}");
}

fn get_monkeys() -> Vec<Monkey> {
    let m0 = Monkey {
        items : vec![72, 64, 51, 57, 93, 97, 68],
        op : MnkOp::Mul(19),
        test : 17,
        true_target: 4,
        false_target: 7
    };

    let m1 = Monkey {
        items : vec![62],
        op : MnkOp::Mul(11),
        test : 3,
        true_target: 3,
        false_target: 2
    };

    let m2 = Monkey {
        items : vec![57, 94, 69, 79, 72],
        op : MnkOp::Add(6),
        test : 19,
        true_target: 0,
        false_target: 4
    };

    let m3 = Monkey {
        items : vec![80, 64, 92, 93, 64, 56],
        op : MnkOp::Add(5),
        test : 7,
        true_target: 2,
        false_target: 0
    };

    let m4 = Monkey {
        items : vec![70, 88, 95, 99, 78, 72, 65, 94],
        op : MnkOp::Add(7),
        test : 2,
        true_target: 7,
        false_target: 5
    };

    let m5 = Monkey {
        items : vec![57, 95, 81, 61],
        op : MnkOp::Sq,
        test : 5,
        true_target: 1,
        false_target: 6
    };

    let m6 = Monkey {
        items : vec![79, 99],
        op : MnkOp::Add(2),
        test : 11,
        true_target: 3,
        false_target: 1
    };

    let m7 = Monkey {
        items : vec![68, 98, 62],
        op : MnkOp::Add(3),
        test : 13,
        true_target: 5,
        false_target: 6
    };

    vec![m0,m1,m2,m3,m4,m5,m6,m7]
}

/*
Monkey 0:
  Starting items: 72, 64, 51, 57, 93, 97, 68
  Operation: new = old * 19
  Test: divisible by 17
    If true: throw to monkey 4
    If false: throw to monkey 7 */
struct Monkey {
    items: Vec<i64>,
    op: MnkOp,
    test: i64,
    true_target: usize,
    false_target: usize
}

enum MnkOp {
    Add(i64),
    //Div(i64),
    Mul(i64),
    Sq
}