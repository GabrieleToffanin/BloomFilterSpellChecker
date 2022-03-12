# BloomFilterSpellChecker

Usage of bloom-filter for saving in a fast and memory-saving way a string or int. 
Uses MurMur3 algorithm for Hashing Written in C#. 
Bloom-Filter has some not-solvable false positives, about the collection containing a never-inserted word or number. That's due to his Array-Cell possible overloading. Tryed to resolve this by using best Length and Best hashing times ( Math equations are writte in Xaml-Notation in the code).


Implementation of MurMur3 non-encryption Algorithm, actually with some issues that needs to be fixed, and still need to implement int hashing and checking into the alghorithm. Thanks for the public MurMur3.   
