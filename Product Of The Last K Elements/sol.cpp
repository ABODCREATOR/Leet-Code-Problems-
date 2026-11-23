class ProductOfNumbers {
private:
    vector <int> pref;
public:
    ProductOfNumbers() {
        pref.push_back(1);
        // This is The First Prefix Product Must Be To Omitting Zeros after multiplication
    }
    
    void add(int num) {
        if(!num){pref.clear(); pref.push_back(1); return;}
        pref.push_back(num * pref.back());
    }
    
    int getProduct(int k) {
      
        int s = pref.size();
        if (s <= k)return 0;
        return pref.back() / pref[s - 1 - k];
    }
    
};

/**
 * Your ProductOfNumbers object will be instantiated and called as such:
 * ProductOfNumbers* obj = new ProductOfNumbers();
 * obj->add(num);
 * int param_2 = obj->getProduct(k);
 */
