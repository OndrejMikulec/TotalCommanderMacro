# Total Commander Macro
Macro for managing tabs in Total Commander. Macro can close all unlocked tabs and add more definned tabs in locked or unlocked state. 

TODO: solve cancel, no TC running - myTotalCommander.cs 44-60
      form 1 start position - centre screen
 
Valid arguments example:

"-Clear" "-LU c:\Users\Ondra\Documents\SharpDevelop Projects" "-RU c:\Users\Ondra" "-RL c:\Users\Ondra\Documents\SharpDevelop Projects"
 
Must have "" 

Target folder without \ at the end 

-LUP left table unlocked push

-RUA right table unlocked add 

-LLP left table lock tab push

-RLA right table lock tab add

-Clear clear all unlocked tabs 

 
For keep TC window size and position, you must first once save position in TC configuration menu. 
 
Macro is looking for standart TC and ini file locations. You can difine your locations in files INIDefinedPath.ini and TCDefinedPath.ini 


