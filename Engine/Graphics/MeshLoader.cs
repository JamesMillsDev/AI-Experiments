using System.Numerics;
using Assimp;
using Assimp.Configs;
using Raylib_cs;
using AssimpMesh = Assimp.Mesh;
using AssimpScene = Assimp.Scene;
using Mesh = Raylib_cs.Mesh;

namespace Engine.Graphics
{
    public static class MeshLoader
    {
        public static List<Mesh> CreateFromAssimp(string file)
        {
            List<Mesh> meshes = [];

            AssimpContext importer = new();
            importer.SetConfig(new GlobalScaleConfig(1f));
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            AssimpScene scene = importer.ImportFile(file);

            foreach (AssimpMesh sceneMesh in scene.Meshes)
            {
                Mesh mesh = new()
                {
                    VertexCount = sceneMesh.Vertices.Count
                };

                mesh.AllocVertices();
                Span<Vector3> vertices = mesh.VerticesAs<Vector3>();
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Vector3(
                        sceneMesh.Vertices[i].X, sceneMesh.Vertices[i].Y, sceneMesh.Vertices[i].Z
                    );
                }

                if (sceneMesh.HasNormals)
                {
                    mesh.AllocNormals();

                    Span<Vector3> normals = mesh.NormalsAs<Vector3>();
                    for (int i = 0; i < normals.Length; i++)
                    {
                        normals[i] = new Vector3(
                            sceneMesh.Normals[i].X, sceneMesh.Normals[i].Y, sceneMesh.Normals[i].Z
                        );
                    }
                }

                if (sceneMesh.HasTangentBasis)
                {
                    mesh.AllocTangents();

                    Span<Vector3> tangents = mesh.TangentsAs<Vector3>();
                    for (int i = 0; i < tangents.Length; i++)
                    {
                        tangents[i] = new Vector3(
                            sceneMesh.Tangents[i].X, sceneMesh.Tangents[i].Y, sceneMesh.Tangents[i].Z
                        );
                    }
                }

                if (sceneMesh.HasTextureCoords(0))
                {
                    mesh.AllocTexCoords();

                    Span<Vector2> texCoords = mesh.TexCoordsAs<Vector2>();
                    for (int i = 0; i < texCoords.Length; i++)
                    {
                        texCoords[i] = new Vector2(
                            sceneMesh.TextureCoordinateChannels[0][i].X,
                            sceneMesh.TextureCoordinateChannels[0][i].Y
                        );
                    }
                }

                meshes.Add(mesh);
                Raylib.UploadMesh(ref mesh, true);
            }

            return meshes;
        }
    }
}