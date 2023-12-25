import sympy
import numpy as np

def day24_part2(lines):
  array = np.array([line.replace('@', ',').split(',')
                    for line in lines], np.int64)  
  
  print(array[0])
  #(p0,p1,p2), (v0,v1,v2),  (t0,t1,t2), 
  p, v, t = (sympy.symbols(f'{ch}(:3)') for ch in 'pvt')

  equations = [
      array[i, j] + t[i] * array[i, 3 + j] - p[j] - v[j] * t[i]
      for i in range(3) for j in range(3)
  ]
  for e in equations:
     print(e)
  result = sympy.solve(equations, (*p, *v, *t))
  print(result)
  total = sum(result[0][:3])
  print(total)
  return

with open ("../input/y23/d24.txt") as f:
    lines = [line.rstrip() for line in f]
    result = day24_part2(lines)
  