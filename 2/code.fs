0 value inputFile
s" ./input.txt" r/o open-file throw to inputFile

create max_numbers s" red" 2, 12 , s" blue" 2, 14 , s" green" 2, 13 ,

create input_buffer 1024 allot

: str_in ( str max_usize substr usize -- flag )
    { str max_usize substr usize }
    max_usize usize < if false exit then

    str usize substr usize compare 0= if true else false then
;

: str_strip ( str usize -- str2 usize2 ) 
    dup 0 ?do
        over i + c@ BL <> if i - swap i + swap unloop exit then
    loop 

;

: str_print_array ( str_array size  -- )
    0 ?do
        type cr
    loop
;

: str>s ( str usize -- s )
    0 s>d 2swap
    >NUMBER 2drop
    d>s
;

: str_split ( str usize separator_str usize2 -- [occurence_addr usize]*n n )
    { str usize separator_str usize2 }
    1
    str \ last pos
    usize usize2 - 0 ?do
        str i + 
        usize i - 
        separator_str usize2
        str_in if
            dup str i + swap - \ set size of previous string
            rot 1 + 
            str i + usize2 + \ new position
            usize2 1 +
        else
            1
        then
    +loop
    dup str usize + swap - \ set size of last string
    rot
;

create current_round 42 , 69 , 0 ,

: reset_round 
    0 current_round !
    0 current_round 1 cells + !
    0 current_round 2 cells + !
;

: add_to_round ( count index -- )
    current_round swap cells + tuck @ ( address count current_content )
    max swap !
;

: get_color_index 
    3 0 ?do
        2dup max_numbers i 3 * cells + 2@ 
        str_in if
            2drop i leave
        then
    loop
;

: count_color ( color_str usize -- count index )
    s"  " str_split drop
    get_color_index -rot
    str_strip str>s
    swap
;

: print_round_results 

    3 0 ?do
        max_numbers 3 i * cells + 2@ type ." : "
        current_round i cells + @ . ." , "
    loop
;

: parse_round ( round_description_str usize -- ) 
    \ 2dup type cr
    s" ," str_split
    0 ?do
        str_strip
        count_color
        add_to_round
    loop
    \ print_round_results cr

;

: is_round_possible ( -- flag )
    3 0 ?do
        max_numbers i 3 * 2 + cells + @
        current_round i cells + @
        < if false unloop exit then
    loop
    true
;

: get_round_power 
    1
    3 0 ?do
        current_round i cells + @ *
    loop 
;

0 value possible_games

: main 
    begin
        input_buffer 1024 inputFile read-line throw
        0<> while ( line_size )
         
        input_buffer swap s" :" str_split drop 2swap
        s"  " str_split drop 2nip str>s 
        ." testing game " . ." : "
        s" ;" str_split
        reset_round
        0 do
            parse_round
        loop
        get_round_power dup . cr
        possible_games + to possible_games 
    repeat
    ." Total power : " possible_games . cr
;

main