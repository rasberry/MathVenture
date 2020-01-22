# SequenceGen #
Playground for number sequence generators. The main motivation is to experiment with
algorithms that can continuously produce digits of infinite series.

## References ###
1. Neal R. Wagner, "pi by continued fraction, how it works", http://www.cs.utsa.edu/~wagner/pi/ruby/pi_works.html
1. Jeremy Gibbons, "Unbounded Spigot Algorithms for the Digits of Pi", https://www.cs.ox.ac.uk/jeremy.gibbons/publications/spigot.pdf
1. Robert Nemiroff, "RJN's More Digits of Irrational Numbers Page", https://apod.nasa.gov/htmltest/rjn_dig.html
1. "The world of Pi - Spigot algorithm", http://pi314.net/eng/goutte.php
1. "Calculation of the Digits of pi by the Spigot Algorithm of Rabinowitz and Wagon", https://www.cut-the-knot.org/Curriculum/Algorithms/SpigotForPi.shtml
1. "PiFast : the fastest program to compute pi", http://numbers.computation.free.fr/Constants/PiProgram/pifast.html

## Usage ##
```
SequenceGen (sequence) [options]
Options:
 -h / --help            Show this help
 -b (number)            Output number in given base (if generator supports it)
 -d (number)            Number of digits to print (default 1000)
 -d-                    Keep printing numbers until process is killed
 -f (file)              Write digits to a file instead of standard out
 -v                     Show progress bar and stats
 -n                     Insert newlines periodically
 -nw                    Number of characters between newlines (default 80)

Sequences:
 1. CofraPi             Continued fraction expansion of pi
 2. GibPi               Expansion of pi using Gibbons spigot method
 3. GibPi2              Expansion of pi using Gibbons spigot method
 4. CofraE              Continued fraction expansion of e
 5. CofraE2             Continued fraction expansion of e
 6. CofraPhi            Continued fraction expansion of phi
 7. CofraSqrt2          Continued fraction expansion of Squre Root of 2
```

## TODO ##
* add stats output / progress bar
  * finite number - progress bar, digits/sec, memory useage
  * infite number - #of digits, digits/sec, memory usage