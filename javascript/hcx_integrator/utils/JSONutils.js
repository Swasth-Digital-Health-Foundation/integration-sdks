function decodeBase46code(encodedString) {
  let decodedBytes = Buffer.from(encodedString, "base64");
  let decodedString = decodedBytes.toString();
  return deserialize(decodedString, clazz);
}
function deserialize(decodedString, clazz) {
  return JSON.parse(decodedString);
}
module.export = decodeBase46code;
