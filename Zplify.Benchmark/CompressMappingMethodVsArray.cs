using BenchmarkDotNet.Attributes;

namespace Zplify.Benchmark
{
    public class CompressMappingMethodVsArray
    {
        [Benchmark]
        public string[] CreateHexCompressionMapping()
        {
            var returnResult = new string[419];
            const string capitalLetters = "GHIJKLMNOPQRSTUVWXY";
            const string lowercaseLetters = "ghijklmnopqrstuvwxyz";
            var returnIndex = 0;
            for (var i = 0; i < capitalLetters.Length; i++)
                returnResult[returnIndex++] = new string(capitalLetters[i], 1);
            for (var i = 0; i < lowercaseLetters.Length; i++)
            for (var j = 0; j < capitalLetters.Length; j++)
            {
                if (j % 20 == 0) returnResult[returnIndex++] = new string(lowercaseLetters[i], 1);
                returnResult[returnIndex++] = new string(new[] {lowercaseLetters[i], capitalLetters[j]});
            }

            return returnResult;
        }

        [Benchmark]
        public string[] InitializeHexCompressionMapping() => new [] {"G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","g","gG","gH","gI","gJ","gK","gL","gM","gN","gO","gP","gQ","gR","gS","gT","gU","gV","gW","gX","gY","h","hG","hH","hI","hJ","hK","hL","hM","hN","hO","hP","hQ","hR","hS","hT","hU","hV","hW","hX","hY","i","iG","iH","iI","iJ","iK","iL","iM","iN","iO","iP","iQ","iR","iS","iT","iU","iV","iW","iX","iY","j","jG","jH","jI","jJ","jK","jL","jM","jN","jO","jP","jQ","jR","jS","jT","jU","jV","jW","jX","jY","k","kG","kH","kI","kJ","kK","kL","kM","kN","kO","kP","kQ","kR","kS","kT","kU","kV","kW","kX","kY","l","lG","lH","lI","lJ","lK","lL","lM","lN","lO","lP","lQ","lR","lS","lT","lU","lV","lW","lX","lY","m","mG","mH","mI","mJ","mK","mL","mM","mN","mO","mP","mQ","mR","mS","mT","mU","mV","mW","mX","mY","n","nG","nH","nI","nJ","nK","nL","nM","nN","nO","nP","nQ","nR","nS","nT","nU","nV","nW","nX","nY","o","oG","oH","oI","oJ","oK","oL","oM","oN","oO","oP","oQ","oR","oS","oT","oU","oV","oW","oX","oY","p","pG","pH","pI","pJ","pK","pL","pM","pN","pO","pP","pQ","pR","pS","pT","pU","pV","pW","pX","pY","q","qG","qH","qI","qJ","qK","qL","qM","qN","qO","qP","qQ","qR","qS","qT","qU","qV","qW","qX","qY","r","rG","rH","rI","rJ","rK","rL","rM","rN","rO","rP","rQ","rR","rS","rT","rU","rV","rW","rX","rY","s","sG","sH","sI","sJ","sK","sL","sM","sN","sO","sP","sQ","sR","sS","sT","sU","sV","sW","sX","sY","t","tG","tH","tI","tJ","tK","tL","tM","tN","tO","tP","tQ","tR","tS","tT","tU","tV","tW","tX","tY","u","uG","uH","uI","uJ","uK","uL","uM","uN","uO","uP","uQ","uR","uS","uT","uU","uV","uW","uX","uY","v","vG","vH","vI","vJ","vK","vL","vM","vN","vO","vP","vQ","vR","vS","vT","vU","vV","vW","vX","vY","w","wG","wH","wI","wJ","wK","wL","wM","wN","wO","wP","wQ","wR","wS","wT","wU","wV","wW","wX","wY","x","xG","xH","xI","xJ","xK","xL","xM","xN","xO","xP","xQ","xR","xS","xT","xU","xV","xW","xX","xY","y","yG","yH","yI","yJ","yK","yL","yM","yN","yO","yP","yQ","yR","yS","yT","yU","yV","yW","yX","yY","z","zG","zH","zI","zJ","zK","zL","zM","zN","zO","zP","zQ","zR","zS","zT","zU","zV","zW","zX","zY"};
    }
}