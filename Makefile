build:
	docker build . -t so2_250147_image
	docker create -p 5224:5224 --name=so2_250147 so2_250147_image

run:
	docker start so2_250147
	
stop:
	docker stop so2_250147