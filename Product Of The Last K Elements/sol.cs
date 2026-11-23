public class ProductOfNumbers {
    private List<int> pref;
    public ProductOfNumbers() {
        pref = new List<int> {1};
    }
    
    public void Add(int num) {
        if (num == 0){
            pref.Clear();
            pref.Add(1);
            return;
        }
        pref.Add(pref[pref.Count - 1] * num);
    }
    
    public int GetProduct(int k) {
        if (k == 0)return 1;

        int n = pref.Count;
        if (n <= k)return 0;
        return pref[n - 1] / pref[n - 1 - k];
    }
}

/**
 * Your ProductOfNumbers object will be instantiated and called as such:
 * ProductOfNumbers obj = new ProductOfNumbers();
 * obj.Add(num);
 * int param_2 = obj.GetProduct(k);
 */
