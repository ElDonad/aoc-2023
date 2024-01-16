140 value input_size
0 value inputFile
s" ./input.txt" r/o open-file throw to inputFile

include ../lib/string.fs

create input_buffer input_size dup * cells allot

: 3DUP  ( a b c -- a b c a b c )  DUP 2OVER ROT ;

: to_digit ( char - num )
    48 -
;

: is_digit ( char - flag )
    [ 0 48 + ] LITERAL [ 10 48 + ] LITERAL within 
    0<>
;

: parse_if_digit ( num - [num] flag )
    dup
    is_digit if to_digit true else drop false then  
;

: get_address ( x y -- addr )
    input_size * + cells input_buffer +
;

: get_xy ( x y -- char )
    get_address c@ 
;

: parse_input
    input_size 0 ?do
        input_size 0 ?do
            input_buffer j input_size * i + cells + inputFile key-file swap ! 
        loop
        inputFile key-file drop \ get rid of CR in the file
    loop

;

: dump_buffer 
    input_size 0 ?do
        input_size 0 ?do
            input_buffer j input_size * i + cells + @
        loop
    cr
    loop
;

: get_next_number ( start_x start_y -- [pos_x pos_y] found? )
    input_size swap ?do
        input_size swap ?do
            i j get_xy parse_if_digit if drop i j true unloop unloop exit then
        loop
        0
    loop
    drop false
;

0 value num_length

: get_number_length ( start_x start_y -- length )
    0 to num_length    
    swap input_size swap ?do \ iterate only along x
        i over get_xy is_digit if num_length 1 + to num_length else leave then
    loop
    drop num_length
;

: nstep ( x y amount -- x' y' )
    tuck input_size / +
    -rot input_size mod +
    swap
;

: wide_type ( cell_addr len -- )
    0 ?do
        dup i cells + c@ emit
    loop
;

: wide_str>s ( cell_addr len -- s )
    0 swap
    0 ?do
        10 *
        over i cells + c@ parse_if_digit if + else leave then
    loop
    nip
;

: check_part ( x y -- is_present )
    \ ."    Checking : " 2dup swap . . cr 
    get_xy dup '.' = swap is_digit or if false else true then 
;

: check_line_for_part ( start_x y length -- present )
    rot swap ( y start_x length )
    over + swap ?do
        i over check_part if unloop drop true exit then
    loop
    drop false
;

: is_colliding ( x y length -- flag )
    { x y length }
    x 0> if x 1 - y check_part if true exit then then
    x input_size 1 - < if x length + y check_part if true exit then then

    y 0> if x y 1 - length check_line_for_part if true exit then then
    y input_size 1 - < if x y 1 + length check_line_for_part if true exit then then

    \ check les diagonales
    x 0> if
        y 0> if
            x 1 - y 1 - check_part if true exit then
        then
        y input_size 1 - < if
            x 1 - y 1 + check_part if true exit then
        then
    then

    x input_size 1 - < if
        y 0> if
            x length + y 1 - check_part if true exit then
        then
        y input_size 1 - < if
            x length + y 1 + check_part if true exit then
        then
    then
    false
;

0 value safegard
0 value total_parts
: main
    0 0
    begin
        get_next_number 0<> safegard 100000000000 < and while
        \ 2dup ." Found next number at position : " swap . . ." , "
        2dup get_number_length \ dup ." Number length : " . cr 
        3dup
        is_colliding if
            3dup -rot get_address swap wide_str>s total_parts + to total_parts
            ." colliding " cr
        else ." not colliding " cr
        then
        nstep \ 2dup ." Next position : " swap . . cr
        safegard 1 + to safegard
    repeat
    total_parts . cr
;

parse_input
main