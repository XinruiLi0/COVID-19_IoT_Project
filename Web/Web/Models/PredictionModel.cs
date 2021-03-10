using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tensorflow;
using NumSharp;

namespace Web.Models
{
    public class PredictionModel
    {
        // private var model = LogisticRegression();

        public static void main()
        {
            var nd = np.full(5, 12); //[5, 5, 5 .. 5]
            nd = np.zeros(12); //[0, 0, 0 .. 0]
            nd = np.arange(12); //[0, 1, 2 .. 11]

            // create a matrix
            nd = np.zeros((3, 4)); //[0, 0, 0 .. 0]
            nd = np.arange(12).reshape(3, 4);

            // access data by index
            var data = nd[1, 1];

            // create a tensor
            nd = np.arange(12);

            // reshaping
            data = nd.reshape(2, -1); //returning ndarray shaped (2, 6)

            Shape shape = (2, 3, 2);
            data = nd.reshape(shape); //Tuple implicitly casted to Shape
                                      //or:
            nd = nd.reshape(2, 3, 2);

            // slicing tensor
            data = nd[":, 0, :"]; //returning ndarray shaped (2, 1, 2)
            data = nd[Slice.All, 0, Slice.All]; //equivalent to the line above.

            // nd is currently shaped (2, 3, 2)
            // get the 2nd vector in the 1st dimension
            data = nd[1]; //returning ndarray shaped (3, 2)

            // get the 3rd vector in the (axis 1, axis 2) dimension
            data = nd[1, 2]; //returning ndarray shaped (2, )

            // get flat representation of nd
            data = nd.flat; //or nd.flatten() for a copy

            // interate ndarray
            foreach (object val in nd)
            {
                // val can be either boxed value-type or a NDArray.
            }

            var iter = nd.AsIterator<int>(); //a different T can be used to automatically perform cast behind the scenes.
            while (iter.HasNext())
            {
                //read
                int val = iter.MoveNext();

                //write
                iter.MoveNextReference() = 123; //set value to the next val
                                                //note that setting is not supported when calling AsIterator<T>() where T is not the dtype of the ndarray.
            }

        }
    }
}
