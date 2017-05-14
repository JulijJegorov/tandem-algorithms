import os
import sys
import json
import numpy as np
from cvxopt import matrix, solvers # not part of Anaconda distribution and should be installed via 'pip install cvxopt' in the command window

# disable print in cmd window
sys.stdout = open( os.devnull, 'w' )

# load arguments vector from the text file
filename = sys.argv[ 1 ]
with open( filename ) as data_file:   
    input_args = json.loads( data_file.read() )

P, q, G, h, A, b = [ matrix( np.array( input_args[ 'args' ][ key ] ) ) if isinstance( input_args[ 'args' ][ key ], list ) 
                                                                    else matrix( input_args[ 'args' ][ key ] ) for key in [ 'P', 'q', 'G', 'h', 'A', 'b' ] ]
# run cvxopt quadratic programming solver 
solver = solvers.qp( P, q, G, h, A, b )

# serialize output results dictionary to json string
output_res = json.dumps( { 'x' : np.array( solver['x'] ).tolist(), 'solution' : solver[ 'primal objective' ] } )

# enable print in cmd window
sys.stdout = sys.__stdout__
print output_res