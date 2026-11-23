# 1352. Product of the Last K Numbers

## Problem

Design a data structure that supports two operations:

- `add(num)`: Add the number `num` to the stream.
- `getProduct(k)`: Return the product of the last `k` numbers in the stream.

The key challenge is handling zeros efficiently. We want both operations to run in **O(1)** time.

---

## 1. Intuition

A normal prefix-product array stores:

$$
prefix[i] = a_0 \times a_1 \times \cdots \times a_i
$$

Then the product of a range can be calculated by division:

$$
product(l, r) = \frac{prefix[r]}{prefix[l-1]}
$$

But there is one problem: **division by zero**.

For example, if the stream is:

```text
[3, 0, 2, 5]
```

The prefix products become:

```text
[3, 0, 0, 0]
```

After adding zero, every later prefix product becomes zero, so we can no longer use division to retrieve products.

### The observation

A zero makes the product of any range containing it equal to zero.

Therefore, instead of keeping prefix products across the entire stream, we can **reset the prefix-product list whenever we receive zero**.

We only need prefix products for numbers added after the most recent zero.

---

## 2. Approach: Prefix Products + Reset on Zero

Maintain a list called `prefix`:

- Initially, it contains `1`.
- When `num` is non-zero, append `prefix.back() * num`.
- When `num` is zero, reset the list to `[1]`.

Why do we start with `1`?

Because it acts as the identity element for multiplication. It also makes the first prefix product easy to calculate.

### How does `getProduct(k)` work?

Let:

- `n` be the current size of the prefix list.
- `prefix[n - 1]` be the product of all numbers added since the most recent zero.
- `prefix[n - 1 - k]` be the prefix product before the last `k` numbers.

Then:

$$
getProduct(k) =
\frac{prefix[n-1]}{prefix[n-1-k]}
$$

There is one important condition:

If `n <= k`, the last `k` numbers include a zero, because the prefix list was reset after that zero. Therefore, return `0`.

For completeness, the empty product for `k = 0` is `1`.

---

## 3. Step-by-Step Visualization

Consider these operations:

```text
add(3)
add(0)
add(2)
add(5)
add(4)
getProduct(2)
getProduct(3)
add(8)
getProduct(3)
add(0)
add(2)
add(5)
getProduct(2)
```

### Simulation

The `prefix` list always starts with a sentinel value `1`. A zero clears the previous prefix products.

| Operation | Prefix list after operation | Explanation |
|---|---|---|
| `add(3)` | `[1, 3]` | `1 × 3 = 3` |
| `add(0)` | `[1]` | Reset because zero was added |
| `add(2)` | `[1, 2]` | `1 × 2 = 2` |
| `add(5)` | `[1, 2, 10]` | `2 × 5 = 10` |
| `add(4)` | `[1, 2, 10, 40]` | `10 × 4 = 40` |
| `getProduct(2)` | `[1, 2, 10, 40]` | `40 / 2 = 20` |
| `getProduct(3)` | `[1, 2, 10, 40]` | `40 / 1 = 40` |
| `add(8)` | `[1, 2, 10, 40, 320]` | `40 × 8 = 320` |
| `getProduct(3)` | `[1, 2, 10, 40, 320]` | `320 / 2 = 160` |
| `add(0)` | `[1]` | Reset again |
| `add(2)` | `[1, 2]` | `1 × 2 = 2` |
| `add(5)` | `[1, 2, 10]` | `2 × 5 = 10` |
| `getProduct(2)` | `[1, 2, 10]` | `10 / 1 = 10` |

### Visual example: `getProduct(2)` after adding `[2, 5, 4]`

```text
Numbers:       2       5       4
                \       \       \
Prefix:         1  →    2  →   10  →   40
                |                |
                |                +---- Total product
                +--------------------- Prefix before last 2 numbers

Answer = 40 / 2 = 20
       = 5 × 4
```

### What happens when the requested range crosses a zero?

Suppose the stream is:

```text
[3, 0, 2, 5]
```

After `add(0)`, the prefix list is reset:

```text
Before zero: [1, 3]
After zero:  [1]
Then add 2:  [1, 2]
Then add 5:  [1, 2, 10]
```

Now:

- `getProduct(2)` returns `10`, because the last two numbers are `2` and `5`.
- `getProduct(3)` returns `0`, because the last three numbers are `0`, `2`, and `5`.

The condition `prefix.size() <= k` detects this case.

---

## 4. Code

```cpp [sol1-C++]
class ProductOfNumbers {
private:
    vector<int> pref;

public:
    ProductOfNumbers() {
        pref.push_back(1);
    }

    void add(int num) {
        if (num == 0) {
            pref.clear();
            pref.push_back(1);
            return;
        }

        pref.push_back(num * pref.back());
    }

    int getProduct(int k) {
        if (k == 0) return 1;

        int n = pref.size();

        if (n <= k) return 0;

        return pref.back() / pref[n - 1 - k];
    }
};
```

```python [sol1-Python]
class ProductOfNumbers:

    def __init__(self):
        self.pref = [1]

    def add(self, num: int) -> None:
        if num == 0:
            self.pref = [1]
            return

        self.pref.append(self.pref[-1] * num)

    def getProduct(self, k: int) -> int:
        if k == 0:
            return 1

        n = len(self.pref)

        if n <= k:
            return 0

        return self.pref[-1] // self.pref[n - 1 - k]
```

```csharp [sol1-C#]
public class ProductOfNumbers {
    private List<int> pref;

    public ProductOfNumbers() {
        pref = new List<int> { 1 };
    }

    public void Add(int num) {
        if (num == 0) {
            pref.Clear();
            pref.Add(1);
            return;
        }

        pref.Add(pref[pref.Count - 1] * num);
    }

    public int GetProduct(int k) {
        if (k == 0) return 1;

        int n = pref.Count;

        if (n <= k) return 0;

        return pref[n - 1] / pref[n - 1 - k];
    }
}
```

---

## 5. Complexity Analysis

Let `k` be the number of recent elements requested by `getProduct(k)`.

| Operation | Time Complexity | Explanation |
|---|---:|---|
| Constructor | `O(1)` | Initialize the prefix list with `1` |
| `add(num)` | `O(1)` | One multiplication and one append; resetting after zero is also `O(1)` amortized with the usual dynamic-array implementation |
| `getProduct(k)` | `O(1)` | A constant number of size checks and one division |

**Auxiliary space:** `O(n)` in the worst case, where `n` is the number of consecutive non-zero values since the most recent zero.

---

## 6. Key Takeaways

1. Prefix products allow range products to be calculated using division.
2. Zero breaks the prefix-product technique because division by zero is impossible.
3. Resetting the prefix list after every zero isolates the numbers after the latest zero.
4. The initial value `1` is a sentinel that simplifies the formula and handles products beginning at the first number after a reset.
5. The solution achieves `O(1)` time for both `add` and `getProduct`.

**Main pattern:** Prefix Products + Handling Zeros.

---

## 7. Interview Explanation

"I maintain prefix products of the numbers since the most recent zero. When a zero is added, I reset the prefix list because any product that includes that zero must be zero. For a query of the last `k` numbers, I divide the latest prefix product by the prefix product immediately before those `k` numbers. If the prefix list does not contain enough elements, the requested range must include a zero, so I return zero. This gives constant-time queries and constant-time additions."

