@ECHO OFF
CHCP 65001 > nul

SET PREFIX=InfDev
SET REPOSITORY_NAME=SKit.Email
SET ARCH_FOLDER=..\
SET ARCH_FOLDER2=f:\Res\OneDrive\!GITARC\InfDev\SKit.Email\

SET YYYYMMDD_HHMMSS=%date:~6,4%%date:~3,2%%date:~0,2%_%time:~0,2%%time:~3,2%%time:~6,2%
IF " " == "%YYYYMMDD_HHMMSS:~9,1%" SET YYYYMMDD_HHMMSS=%YYYYMMDD_HHMMSS: =0%
IF " " == "%YYYYMMDD_HHMMSS:~6,1%" SET YYYYMMDD_HHMMSS=%YYYYMMDD_HHMMSS: =0%

SET FILENAME=%PREFIX%!%REPOSITORY_NAME%!%YYYYMMDD_HHMMSS%.zip
SET ARCH_PATH=%ARCH_FOLDER%%FILENAME%

echo "Добавление в индекс текущих файлов ..."
git add --all
echo "Зафиксировать изменения ..."
git commit -m "Step %YYYYMMDD_HHMMSS%"
echo "Архивация репозитария в %ARCH_PATH% ..."
git archive -o %ARCH_PATH% HEAD
copy %ARCH_PATH% %ARCH_FOLDER2%
