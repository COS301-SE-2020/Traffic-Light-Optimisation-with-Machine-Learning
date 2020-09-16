cd Server/aegis/   
mvn clean package
java -jar target/aegis-0.0.1-SNAPSHOT.jar &
cd ..
cd ..
cd ServerAIUnityApp/
./AegisApp.x86_64 -nographics &
cd ..
cd ServerNoAIUnityApp/
./AegisApp.x86_64 -nographics &