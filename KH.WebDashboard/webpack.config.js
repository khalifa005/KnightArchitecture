const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');

module.exports = {
  resolve: {
    extensions: ['.ts', '.js'], // Ensure .ts comes first
  },
  plugins: [
    new BundleAnalyzerPlugin({
      analyzerMode: 'static', // Generates a static HTML file
      reportFilename: 'bundle-report.html', // Name of the report file
      openAnalyzer: true, // Automatically open the report
    }),
  ],
};
