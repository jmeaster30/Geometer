# Geometer
A Geometry Modeling Thingy

I had this idea when I was in my geometry class in college where we had to draw out our problems and this helped us solve them.
However, there was this one problem that broke my brain. We were given an image of a geometric construction and we had to figure out why the conclusion of the construction was wrong. The issue was in how it was drawn. The image we received showed this one angle as less than another angle yet if you went through the problem by hand you would find that angle was always greater than the other angle. Also in college, I took a class on the theory of computation and I had a project on automated theorem solvers and logic programming.

So from those two inspirations, I had the idea of a program that let you build geometric models and query them to learn stuff about the model. My goal in this project is to be able to pick some problems from my copy of Euclid's The Elements and solve them.

I don't expect this to be soemthing that could generate mathematical proofs for theorems to do your math homework for you. My idea of this more of something like logic programming that builds a model of relationships between things and lets you get an intuition for the geometric problem you are working with.

## Running the program

You can clone the repo, navigate into the 'Geometer' folder from the root of this repo, and then just use the command 'dotnet run' and it will build and run Geometer.

This project uses .Net6 so you will need that runtime/sdk to run/compile this program. The GUI is created in GTK which I believe is cross-platform (I have developed this on Linux and will be trying it on Windows someday).

I have not looked into fully installing Geometer onto a machine.

## Samples

There is a folder in this repo with samples. The samples get automatically loaded into the "Samples" menu so you can check them out by clicking on the dropdown menu and selecting a sample.

If you would like to contribute a sample that is interesting, feel free to open a pull request with your sample in the Geometer/Samples folder. The naming pattern I have used (and is important for loading into the menu) is "some_name.geo". Note, the filename is in all **lowercase**, words are separated by an **underscore** ("_"), and the file extension is **geo**.

Quality | Filename | Explanation
--------|----------|------
Good | Geometer/Samples/regular_triangle.geo | 
Bad | Geometer/thales_theorem.geo | Not in the 'Geometer/Samples' folder.
Bad | Geometer/Samples/ThalesTheorem.geo | Used capitals in filename. The two words are not separated by an underscore (but this is not necessary it is more of how you want the sample to show up in the GUI)
Bad | Geometer/Samples/thales_theorem.txt | File extension is 'txt' instead of 'geo'
Good | Geometer/Samples/thales_theorem.geo | 