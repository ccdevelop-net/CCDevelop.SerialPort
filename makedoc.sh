#/bin/bash
echo Remove all documentation
rm -rf doc/html
rm -rf doc/latex
echo Generate documentation by Doxygen
doxygen serport.doxygen
read -p "Press enter to generate PDF or Ctrl-C to end"
cd doc/latex
make
cp refman.pdf ../../"Reference Manual CCDevelop.SerialPort.pdf"
cd ../..


