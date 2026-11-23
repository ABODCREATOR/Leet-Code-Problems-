C++
Intuition
To Make Sure That All A's Comes Before B's Note The Lexicographically order of A is shorter than B But to ensure that all of cases true we must sort a string lexicographically

Approach
We Know the Whole Probability of different characters in whole cases consists of a's and b's only and we must check that the whole string is sorted lexicographically using function is_sorted in cpp

Complexity
Time complexity: O(n)
Space complexity: O(1)
Code
class Solution {
public:
    bool checkString(string s) {
        return is_sorted(s.begin() , s.end());
    }
};
 
