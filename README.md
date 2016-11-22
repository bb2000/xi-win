# xi-win
A Windows front end for the Xi Editor.

## Installing
After building the application with Visual Studio, you will need to put 2 files in the same directory as the .exe produced.

These are:
*   xi-core.exe (The Xi Core executable which can be built following the instructions on the xi-editor repo)
*   README.md (The help file which you are currently reading and which shows up when you start the editor)

## Help
This editor works much like a usual editor, with the buttons at the top left allowing you to open a file, save a file, open and close tabs.
Much like Vim, you can enter a command mode by pressing Left Ctrl. Available commands are o for open and s to save.
To get back to editing, press Escape.

## Known issues/bugs
*   The application sometimes crashes randomly.
*   Files over 1000 lines will not load all the way.
*   Large files can cause massive slow-downs.
*   Tab names are numbers and not file names
*   No error messages for file not found errors
*   Shift+Num does not show special character, but just that number
*   Copy and paste does not work
*   Some keys (such as tab) will not work properly
