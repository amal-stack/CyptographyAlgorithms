[![.NET](https://github.com/amal-stack/CyptographyAlgorithms/actions/workflows/dotnet.yml/badge.svg)](https://github.com/amal-stack/CyptographyAlgorithms/actions/workflows/dotnet.yml)

# Cyptography Algorithms
[*In progess*]

Implementation of cryptography algorithms in C#.

# Symmetric or Secret-key Cryptography

## 1. Shift Cipher (Caesar)

$$
\begin{array}{c}
 p \in \mathbb{Z}_{26} \\
 c \in \mathbb{Z}_{26} \\
 k \in \mathbb{Z}_{26} \\
 e_{k}(p) = (p + k)\pmod{26} \\
 d_{k}(c) = (c - k)\pmod{26}
\end{array}
$$

2. Monoalphabetic Substitution Cipher
3. Rail fence Cipher
4. Polyalphabetic Cipher
5. Permutation Cipher
6. Playfair Cipher
7. Substitution Permutation Network
8. Data Encryption Standard (DES)
9. TripleDES
10. Advanced Encryption Standard (AES)


# Assymetric or Public-key Cryptography
## 1. Diffie-Hellman Key Exchange
1. Let $p$ be a prime number.
2. Let $g$ be the generator of the cyclic group $\mathbb{Z}_p^{*}$.
3. Alice (the sender) chooses $a$ such that $0 < a < p-1$.
4. Bob (the receiver) chooses $b$ such that $0 < b < p-1$.
5. Alice computes $A = g^a$ and sends it to Bob.
6. Bob computes $B = g^b$ and sends it to Alice.
7. Alice receives $B$ and computes $B^a = {(g^b)^a}\pmod p$.
8. Bob receives $A$ and computes $A^b = {(g^a)^b}\pmod p$.

## 2. RSA

* Based on integer factorization (mathematical hard problem):
##### Integer Factorization
Given a large semiprime number $n$, find its two prime factors $p$ and $q$ such that:

$$ n = pq $$

### Setup
1. Select two primes $p$ and $q$.
2. Compute $n = pq$.
3. Compute $\phi(n) = (p - 1)(q - 1)$.
4. Select $e$ such that $\gcd(e,\phi(n)) = 1$.
5. Find $d$ such that $ed = 1\pmod {\phi(n)}$.
  * Public Key: $(e, n)$
  * Private Key: $(d, p, q)$.
  
### Encryption
$$
\begin{array}{c}
m \in \mathbb{Z}_n \quad gcd(m, n) = 1 \\
\end{array}
$$

$$
\begin{align}
c & = E(m) \\
& = {m^e}\pmod n \\
\end{align}
$$

### Decryption
$$
\begin{align}
m & = D(c) \\
& = {c^d}\pmod n \\
& = {m^{ed}}\pmod n \\
\end{align}
$$

## 3. Knapsack Cryptography
### Setup
1. Choose a superincreasing sequence of $n$ integers:

$$ A^\prime = \lbrace  a_1^\prime, a_2^\prime, \dots, a_n^\prime \rbrace $$

2. Choose $m$ such that $m > \sum{a_i}$.
3. Choose $w$ such that $\gcd(m, w) = 1$.
4. Generate public sequence:

$$ A = \lbrace a_1, a_2, \dots, a_n \rbrace $$

$$where, a_i = {w a_i^\prime}\pmod m$$

### Encryption
Message: $X = \lbrace x | x \in \lbrace 0, 1 \rbrace \rbrace$

$$
\begin{align}
c & = E(x) \\
& = \sum_{i=1}^n x_ia_i
\end{align}
$$

### Decryption
1. Find $w^{-1}$ under $\pmod m$.
2. Compute ${w^{-1}c}\pmod m$.
3. Solve the subset sum problem.



## 4. ElGamal Cryptosystem
###### DLP
1. Choose a prime $p$.
2. For cyclic group $\mathbb{Z}_p^{*} = \mathbb{Z}_p - \lbrace 0 \rbrace$, $\alpha$ is the generator.
3. Compute $\beta = {\alpha^a}\pmod p$ where $0 \le a \le p-2$.
4. Given $(\alpha, \beta, p)$ find $a$.

### Setup
1. Choose $p$ such that DLP is hard.
2. For the cyclic group $\mathbb{Z}_p^{*}$ whose generator is $\alpha$, choose such that $0 \le a \le p-2$.
3. Compute $\beta = {\alpha^a}\pmod p$.
* Public Key: $(\alpha, \beta, p)$
* Private Key: $a$.

### Encryption
* Choose $k$ such that $k \in \mathbb{Z}_p^{*}$.
* Message: $x \in \mathbb{Z}_p^{*}$.

$$E(x, k) = (y_1, y_2)$$

where,

$$ y_1 = {\alpha^k} \pmod p $$

$$ y_2 = {x\beta^k} \pmod p $$

### Decryption

$$ \begin{align}
D(y_1, y_2) & = {y_2(y_1^a)^{-1}}\pmod p \\
& = x
\end{align}$$



## 5. Rabin Cryptosystem
* Based on CRT.
* Invented before RSA.
### Setup
1. Choose two primes $p$ and $q$.
2. Compute $n = pq$.
* Public Key: $(n)$
* Private Key: $(p, q)$

### Encryption
$$\begin{align} 
E_k(x) &= {x^2}\pmod n \\
&= y
\end{align}$$

### Decryption
$$\begin{align}
y &\equiv {x^2}\pmod n \\
x^2 &\equiv y\pmod n \\
\text{ By CRT, } \\
x^2 &\equiv y\pmod p \\
x^2 &\equiv y\pmod q \\
\end{align}$$


## 6. Goldwasser-Micali Cryptosystem
### Setup
1. Choose two large primes $p$ and $q$.
2. Compute $n=pq$.
3. Choose $m$ such that:
$$(\frac{m}{p}) = -1 = (\frac{m}{q})$$
$$m \notin {QR}_p$$
$$m \notin {QR}_q$$

* $m \in {QR}\pmod n$ if $m \in {QR}\pmod p$ and $m \in {QR}\pmod q$ and $n = pq$.
* Public Key: $(m, n)$
* Private Key: $(p, q)$

### Encryption
* Plaintext: $X = \lbrace x_i | x_i \in \lbrace 0, 1 \rbrace \rbrace$
* Ciphertext: $y \in \mathbb{Z}_n^{*}$

For each bit $x_i \in X$: 

Choose a random number $r$ such that $r \in \mathbb{Z}_n^{*}$.

$$\begin{align}
y &= E_k(x_i) \\
&= {m^{x_i}r^2}\pmod n \\
&= c_i
\end{align}$$


### Decryption

$$
D_k(y) = 
\begin{cases}
0, & \text{if  } y \in QR_{n} \\
1, & \text{if  } y \notin QR_{n}
\end{cases}
$$


# Digital Signature Schemes
## 1. RSA Signature Scheme
### Setup
1. Select two primes $p$ and $q$.
2. Compute $n = pq$.
3. Compute $\phi(n) = (p - 1)(q - 1)$.
4. Select $e$ such that $\gcd(e,\phi(n)) = 1$.
5. Find $d$ such that $ed = 1\pmod {\phi(n)}$.
  * Public Key: $(e, n)$
  * Private Key: $(d, p, q)$.
  
### Signing
$$
\begin{array}{c}
m \in \mathbb{Z}_n \quad gcd(m, n) = 1 \\
\end{array}
$$

$$
\begin{align}
y & = E_d(m) \\
& = {m^d}\pmod n \\
\end{align}
$$

### Verification
$$
\begin{align}
m & = D_e(y) \\
& = D_e(E_d(m)) \\
& = {y^e}\pmod n \\
& = {m^{ed}}\pmod n \\
\end{align}
$$


## 2. ElGamal Signature Scheme over $\mathbb{Z}_p$

### Setup
1. Choose $p$ such that DLP is hard.
2. For the cyclic group $\mathbb{Z}_p^{*}$ whose generator is $\alpha$, choose such that $0 \le a \le p-2$.
3. Compute $\beta = {\alpha^a}\pmod p$.
* Public Key: $(\alpha, \beta, p)$
* Private Key: $a$.

### Encryption
* Choose $k$ such that $k \in \mathbb{Z}_p^{*}$.
* Message: $x \in \mathbb{Z}_p^{*}$.

$$sign_d(x, k) = (\gamma, \delta)$$

where,

$$\begin{align}
\gamma &= {\alpha^k} \pmod p  \\
\delta &= {(x - ay_1)k^{-1}} \pmod p 
\end{align}$$

### Verification
1. Calculate $\beta^\gamma \gamma^\delta$.
2. Calculate $\alpha^x$.
3. Check if $\beta^\gamma \gamma^\delta = \alpha^x$.




