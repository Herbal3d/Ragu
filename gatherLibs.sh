#! /bin/bash

LIBOMV=../../libopenmetaverse/bin
RSGPROMISE=../../C-Sharp-Promise/bin/Release

# Copy the dll file and the PDB file if it exists
function GetLib() {
    cp "$1/$2" libs
    pdbfile=$1/${2%.dll}.pdb
    if [[ -e "$pdbfile" ]] ; then
        cp "$pdbfile" libs
    fi
}

GetLib "$LIBOMV" "OpenMetaverse.dll"
GetLib "$LIBOMV" "OpenMetaverse.dll.config"
GetLib "$LIBOMV" "OpenMetaverse.XML"
GetLib "$LIBOMV" "OpenMetaverseTypes.dll"
GetLib "$LIBOMV" "OpenMetaverseTypes.XML"
GetLib "$LIBOMV" "OpenMetaverse.StructuredData.dll"
GetLib "$LIBOMV" "OpenMetaverse.StructuredData.XML"

GetLib "$LIBOMV" "log4net.dll"

GetLib "$RSGPROMISE" "RSG.Promise.dll"

