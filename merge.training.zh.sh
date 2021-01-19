#!/bin/sh

rm /cygdrive/c/temp/training.zh.txt

function process_folder {
  pushd "$1"
  find . -name '*.txt' | xargs -d '\n' cat >> /cygdrive/c/temp/training.zh.txt
  find . -name '*.json' | xargs -d '\n' cat >> /cygdrive/c/temp/training.zh.txt
  popd
}

process_folder /cygdrive/c/Users/Hzj_jie/Documents/git/training-data/zh/People\'s\ Newspaper/
process_folder /cygdrive/f/My\ Books/pending/disk1/
process_folder /cygdrive/f/My\ Books/pending/disk4/
process_folder /cygdrive/c/Users/Hzj_jie/Documents/git/training-data/zh/CCTV\ News/2016-2019/
