#!/bin/bash
sed -i '/<<<<<<< HEAD/d' README.md
sed -i '/=======/d' README.md
sed -i '/>>>>>>> origin\/master/d' README.md
