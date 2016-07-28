
tf undo /recursive . /noprompt
tf get * /recursive /force
git reset --hard origin/master
git pull

