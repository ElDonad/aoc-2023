(
    Librairie simple pour créer des tableaux dynamiques
    Supporte seulement les cells seules pour le moment.
)

\ vector : capacity, cell_size, current_size, data_pointer
: vector ( cell_size initial_total_size -- vector ) create 2dup , , 0 , * cells allocate throw , ;

: vec_new ( cell_size -- vector ) 8 vector ;

: vec_get_capacity ( vector_address -- total_size )
    @
;

: vec_set_capacity ( new_capacity vector -- )
    !
;

: vec_get_cell_size ( vector -- cell_size )
    1 cells + @
;

: vec_get_size ( vector_address -- size )
    2 cells + @
;

: _vec_set_size ( new_size vector -- )
    2 cells + !
;

: _vec_get_data_ptr
    3 cells + @
;

: _vec_get_data_ptr_index ( index vector -- address )
    dup -rot vec_get_cell_size * cells swap _vec_get_data_ptr +
;

: _vec_set_data_ptr
    3 cells + !
;

: data_set ( ...data address count -- )
    0 do
        tuck !
        cell +
    loop
    drop
;

: data_get ( address count -- ...data )
    0 do
        dup @ swap
        cell + 
    loop
    drop
;

\ Sets a value in the vector's data space. Should not be used, as it does not do any safety check.
: _vec_set ( ...data index vector -- )
    tuck _vec_get_data_ptr_index swap  vec_get_cell_size data_set
;

: vec_is_within_capacity ( index vector -- flag )
    vec_get_capacity <
;

: vec_get ( index vector -- ...value )
    tuck _vec_get_data_ptr_index swap vec_get_cell_size data_get
;

: vec_set ( ...data index vector -- success )
    2dup vec_is_within_capacity 0= if
        -1 throw
    then 
    _vec_set 0
;

: vec_is_full ( vector -- flag )
    dup vec_get_capacity swap vec_get_size =
;


\ doubles the vector capacity
: vec_expand ( vector -- )
    dup dup
    vec_get_size 2 * swap vec_get_cell_size * cells 
    over _vec_get_data_ptr swap resize throw 
    swap _vec_set_data_ptr
;

: vec_push_back ( cell vector -- ) 
    dup vec_is_full if
        dup vec_expand 
    then
    dup dup vec_get_size 1 + swap _vec_set_size
    dup vec_get_size 1 - swap _vec_set
;

2 vec_new coucou
: test 
    32 0 do
        i i 2 * coucou vec_push_back
    loop
    .s cr
    30 coucou vec_get . .
;
test 
