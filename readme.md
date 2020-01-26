# MathVenture #
A Collection of math projects.

## Usage ##
```
Usage MathVenture (aspect) [options]
Options:
 -h / --help                  Show full help
 (aspect) -h                  Aspect specific help
 --aspects                    List possible aspects

1. SequenceGen (sequence) [options]
 A set of algorithms that can continuously produce digits of infinite series
Options:
 -b (number)                  Output number in given base (if generator supports it)
 -d (number)                  Number of digits to print (default 1000)
 -d-                          Keep printing numbers until process is killed
 -f (file)                    Write digits to a file instead of standard out
 -v                           Show progress bar and stats
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

## TODO ##
