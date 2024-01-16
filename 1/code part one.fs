0 value inputFile
s" ./input.txt" r/o open-file throw to inputFile

create input_buffer 512 allot

: is_digit? ( num - flag )
    [ 0 48 + ] LITERAL swap [ 9 48 + ] LITERAL within 0= 
;

: to_digit ( char - num )
    48 -
;

: process_line ( size -- first_number last_number )
    dup
    0 swap \ prepare index for the for loop
    0 DO
        dup 1 + swap
        input_buffer + c@
        dup
        is_digit? IF
            nip to_digit leave \ get the first number on the stack, remove the loop argument
        else
            drop \ remove the duplicate
        then
    LOOP

    swap dup 1 - swap \ get the buffer address and the counter
    0 swap DO
        dup 1 - swap
        input_buffer + c@
        dup
        is_digit? IF
            nip to_digit leave \ get the first number on the stack, remove the loop argument
        else
            drop \ remove the duplicate
        then
    -1 +LOOP
;

VARIABLE total
0 total !
: main
BEGIN
    input_buffer 512 inputFile read-line throw
    0<> WHILE
    process_line 
    swap 10 * + total @ + total !
REPEAT
total @ .
;

main
