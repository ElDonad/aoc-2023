0 value inputFile
s" ./input.txt" r/o open-file throw to inputFile

create input_buffer 512 allot

create written_digits s" one" 2, s" two" 2, s" three" 2, s" four" 2, s" five" 2, s" six" 2, s" seven" 2, s" eight" 2, s" nine" 2,

: to_digit ( char - num )
    48 -
;

: parse_if_digit ( num - [num] flag )
    dup
    [ 0 48 + ] LITERAL swap [ 9 48 + ] LITERAL within 
    0= if to_digit true else drop false then  
;

: compare_letter_digit ( input_char input_usize num_char num_usize - flag )
    { input_char input_usize num_char num_usize }
    input_usize num_usize < if false exit then \ if the input string is smaller than the checked digit, impossible

    input_char num_usize num_char num_usize compare 0= if true else false then

;

: is_letter_digit ( input_char input_usize - [digit] is_digit )
    9 0 ?do
        2dup 
        written_digits i 2 * cells + 2@
        compare_letter_digit if 2drop i 1 + true unloop exit then
    loop
    2drop false
;

: get_index ( size direction i --  i' )
    swap
    case 
        -1 of - 1 - endof
        1 of nip endof
    endcase
;

: substring ( size direction i -- char_addr size )
    { size direction index }
    direction
    case
        -1 of 
            input_buffer size + 1 - index -  
            index 1 +
        endof
        1 of 
            input_buffer index + 
            size index -
        endof
    endcase
;

: process_line ( size direction -- first_number )
    over 0 ?do
        2dup i get_index

        input_buffer + c@ 
        parse_if_digit if leave then
        2dup i
        substring
        is_letter_digit
         if leave then 
    loop
    nip nip
;

VARIABLE total
0 total !
: main
begin
    input_buffer 512 inputFile read-line throw
    0<> while
    dup
    1 process_line swap
    -1 process_line
    .s cr
    swap 10 * + total @ + total !
repeat
total @ .
;

main