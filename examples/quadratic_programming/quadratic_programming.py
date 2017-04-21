import os
import sys
import json
import numpy as np
from cvxopt import matrix, solvers

# disable print in cmd window
sys.stdout = open( os.devnull, 'w' )

# load argument vector
input_args = json.loads( sys.argv[ 1 ] )
P, q, G, h, A, b = [ matrix( np.array( input_args[ 'args' ][ key ] ) ) if isinstance( input_args[ 'args' ][ key ], list ) 
                                                                    else matrix( input_args[ 'args' ][ key ] ) for key in [ 'P', 'q', 'G', 'h', 'A', 'b' ] ]
# run cvxopt quadratic programming solver 
solver = solvers.qp( P, q, G, h, A, b )

# serialize output results dictionary to json string
output_res = json.dumps( { 'x' : np.array( solver['x'] ).tolist(), 'solution' : solver[ 'primal objective' ] } )

# enable print in cmd window
sys.stdout = sys.__stdout__
print output_res