@echo off

git pull local HEAD:main
git pull gitee HEAD:main

git push local HEAD:main
git push gitee HEAD:main

echo synchronization of this code repository job done!