# Use Node.js image for building and serving the Angular app
FROM node:18.19.1 AS build

# Set working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json to install dependencies
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire Angular app source code
COPY . .

# Expose the development server port
EXPOSE 4200

# Default command to run the Angular app in development mode
CMD ["npm", "start", "--", "--host", "0.0.0.0"]

