import sys
import json
import numpy as np
from sklearn.decomposition import PCA

# load arguments vector from the text file
filename = sys.argv[ 1 ]
with open( filename ) as data_file:   
    input_args = json.loads( data_file.read() )

cov, n, seed = [ input_args[ 'args' ].get( key ) for key in [ 'cov', 'n', 'seed' ] ]

np.random.seed( int( seed ) )

# sample from multivariate normal distribution with mean [ 0, 0 ] and covariance matrix 'cov'
data = np.random.multivariate_normal( np.zeros( 2 ), cov, size = int( n ) )

# compute eigenvectors of covariance matrix 'cov'
W_true = np.linalg.eig( cov )[ 1 ]

# principal component analysis of 'data'
W_pca = PCA().fit( data ).components_

# serialize output results dictionary to json string
output_res = json.dumps( { 'data' : data.tolist(), 'W_true' : W_true.tolist(), 'W_pca' : W_pca.tolist() } )

print output_res



