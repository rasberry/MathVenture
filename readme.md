# MathVenture #
A Collection of math projects.

## Usage ##
```
Usage MathVenture (aspect) [options]
Options:
 -h / --help                  Show full help
 (aspect) -h                  Aspect specific help
 --aspects                    List possible aspects
 --debug                      Show debug messages (implies -v)
 -v                           Show extra information

1. SequenceGen (sequence) [options]
 A set of algorithms that can continuously produce digits of infinite series
Options:
 -b (number)                  Output number in given base (if generator supports it)
 -d (number)                  Number of digits to print (default 1000)
 -d-                          Keep printing numbers until process is killed
 -f (file)                    Write digits to a file instead of standard out
 -p                           Show progress bar and stats
 -n                           Insert newlines periodically
 -nw                          Number of characters between newlines (default 80)

Sequences:
 1. CofraPi                   Continued fraction expansion of pi
 2. GibPi                     Expansion of pi using Gibbons spigot method
 3. GibPi2                    Expansion of pi using Gibbons spigot method
 4. BppPi                     Expansion of pi using Bailey-Borwein-Plouffe method
 5. CofraE                    Continued fraction expansion of e
 6. CofraE2                   Continued fraction expansion of e
 7. CofraPhi                  Continued fraction expansion of phi
 8. CofraSqrt2                Continued fraction expansion of Squre Root of 2

2. AltMat (function) [number] [options]
 Implementations of some math functions using various numeric methods
Options:
 -b (number)                  Series start number
 -e (number)                  Series end number
 -s (number)                  Series step amount
 -a (number)                  Accuracy (for functions that support it)
 -io                          Print both input and output

Functions:
  1. CordicSin                Sin using CORDIC (for COordinate Rotation DIgital Computer)
  2. CordicCos                Cos using CORDIC (for COordinate Rotation DIgital Computer)
  3. SpecrumSin               ZX Spectrum Calculator SIN X function
  4. Sin5                     Sin using Chebyshev mapping method
  5. Sin6                     Sin using Geometric Product expansion sin(x) = x*PROD(k=[1,Inf](1-(x/(k*PI)^2)))
  6. SinFdlibm                Sin function from fdlibm/k_sin.c
  7. SinSO                    Sin function from Stack Overflow 'How does C compute sin() and other math functions?'
  8. SinSO2                   Sin function from Stack Overflow 'How does C compute sin() and other math functions?'
  9. SinTaylor                Sin function using trucated taylor series
 10. SinXupremZero            Sin function by XupremZero
 11. CosXupremZero            Cos function by XupremZero

3. PrimeGen (gen|bits|bitsimg) [options]
gen                           Generates a sequence of prime numbers and outputs one per line
 -t (type)                    Type of generator to use (leave empty to list the types)
 -s (number)                  Starting number for the generator (default 2)
 -e (number)                  Ending number for the generator (default 100)
 -f (file)                    Optional text file to store primes

4. Factors (number) [options]
 -m (method)                  Use another method (default Basic)

Methods:
 1. Basic
 2. Sqrt
 3. Parallel

5. LFSR [options]
 -b (number)                  The bit count to produce a 2^(bitcount)-1 sequence [1-63]
 -m (number)                  Max number to allow for the sequence (-b and -m should not be used together)
 -x                           Use Xilinx taps instead of random ones (ignored if -t is specified)
 -t (number)                  A primitive polynomial for use as taps
 -i (number)                  The initial state for the LFSR (defaults to 1)
```
## References ###
### SequenceGen ###
1. Neal R. Wagner, "pi by continued fraction, how it works"
   http://www.cs.utsa.edu/~wagner/pi/ruby/pi_works.html
1. Jeremy Gibbons, "Unbounded Spigot Algorithms for the Digits of Pi"
   https://www.cs.ox.ac.uk/jeremy.gibbons/publications/spigot.pdf
1. Robert Nemiroff, "RJN's More Digits of Irrational Numbers Page"
   https://apod.nasa.gov/htmltest/rjn_dig.html
1. "The world of Pi - Spigot algorithm"
   http://pi314.net/eng/goutte.php
1. "Calculation of the Digits of pi by the Spigot Algorithm of Rabinowitz and Wagon"
   https://www.cut-the-knot.org/Curriculum/Algorithms/SpigotForPi.shtml
1. "PiFast : the fastest program to compute pi"
   http://numbers.computation.free.fr/Constants/PiProgram/pifast.html
1. David H. Bailey, "BBP Code Directory"
   https://www.experimentalmath.info/bbp-codes/

### AltMath ###
1. Ian Logan; Frank O'Hara, "The Complete Spectrum ROM Disassembly"
   http://freestuff.grok.co.uk/rom-dis/

### PrimeGen ###
1. "The AKS "PRIMES in P" Algorithm Resource
   http://fatphil.org/maths/AKS/#Implementations
1. Phillip Mates, "3 Primality Proving Implementations: ECPP, AKS, and a hybrid of the two"
   https://github.com/philomates/ecpp-aks-primality-proving
1. Futility Closet, "Pascal’s Primes"
   https://www.futilitycloset.com/2015/12/29/pascals-primes/
1. Miller–Rabin primality test
   https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test

### LFSR ###
1. GF(2) is the unique Galois field with two elements
   https://en.wikipedia.org/wiki/GF%282%29
1. Arndt, Jörg; "FXT: a library of algorithms"
   https://www.jjj.de/fxt/fxtpage.html

## TODO ##
