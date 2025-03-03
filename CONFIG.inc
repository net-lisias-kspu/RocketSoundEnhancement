source ../CONFIG.inc

PACKAGE=RocketSoundEnhancements
TARGETDIR="$PACKAGE"
TARGETBINDIR="$TARGETDIR"
PROJECTSDIR="$PACKAGE/"
VERSIONFILE="$PACKAGE.version"
DLLS="$PACKAGE""
PD_DLLS=""
GD_DLLS=""
GD_PRIORITY=""

PROJECT_BRANCH=`git branch | grep \* | cut -d ' ' -f2`
if [ "$PROJECT_BRANCH" == "master" ] ; then
    # Release mode
    PROJECT_STATE=""
else
    # Note: Add a leading dash when this value is present!
    PROJECT_STATE=""
fi

VERSION=$( cat $PACKAGE.version | tr '\n' ' ' | sed -n -E 's/^.*?"VERSION\":\{.*"MAJOR":([0-9]+?),.*?"MINOR":([0-9]+?),.*?"PATCH":([0-9]+?),.*?"BUILD":([0-9]+?).*?\}.*$/\1.\2.\3.\4/p' )
