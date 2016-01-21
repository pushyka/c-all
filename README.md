# Chess
<pre>This is a chess implementation I have been working on for fun. I wanted to write an AI chess engine and 
figured I would write the main chess program first. So far this just allows two people to play against each 
other. The chess program currently receives all its inputs from the Windows Forms display. I intend to write 
a View using WPF and an AI engine which will also be able to provide some inputs for the chess game.

 _________                ____________             _________
| View 1. |------------->|            |-----3.--->|         |       -Move Process-
|         |              | controller |           | model   |   1. A pair of coordinates are generated
|    5.   |<--------------------------------4.----|         |   2. The pair is evaluated for legality as a move 
|_________|              |____________|           |_________|   3. If legal the move is applied to the model
                           |       ^                            4. The View detects the change
                         __v_______|___                         5. The View updates itself to match the model
                        | 2.           |
                        |  evaluator   |                           
                        |______________|        
                        
View / Interface receives commands from the user, does any intermediate view code processing (eg tints / 
disabling menu) and usually sends a command / input to the controller which does some processing and interacts
with the model. When the model changes it raises an event (notifies) the View with the information about the
change and the View can access the relevant readable properties on the model in order to update itself.

When I planned this approach I thought I was using MVC but while I have multiple models and potentially multiple 
views, the controller doesn't seem to be something I would like to have different copies of. Also the way in 
which the View is updated seems to be different than how MVC should work. Having to use the Invoke->View thread 
procedure just seems overly complicating. I have read that WPF uses a data binding of the view to the model so 
perhaps this is the simplicity I am looking for.

I am intending for the Chess game to follow the FIDE Laws of Chess, including complete promotion, en passant and 
castling. The saving and loading of games will use the Standard Algebraic Notation.</pre>
